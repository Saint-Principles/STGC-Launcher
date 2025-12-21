using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace STGCLauncher
{
    public static class FontManager
    {
        private static PrivateFontCollection _fontCollection;
        private static FontFamily _customFontFamily;

        public static bool Initialize()
        {
            try
            {
                string fontPath = SettingsManager.Settings.LauncherFont;

                if (string.IsNullOrEmpty(fontPath) || !File.Exists(fontPath)) return false;

                _fontCollection = new PrivateFontCollection();
                _fontCollection.AddFontFile(fontPath);

                if (_fontCollection.Families.Length > 0)
                {
                    _customFontFamily = _fontCollection.Families[0];
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load font: {ex.Message}");
                return false;
            }
        }

        public static Font CreateFont(float size, FontStyle style = FontStyle.Regular)
        {
            if (_customFontFamily != null && _customFontFamily.IsStyleAvailable(style))
            {
                return new Font(_customFontFamily, size, style);
            }

            return new Font(SystemFonts.DefaultFont.FontFamily, size, style);
        }

        public static IEnumerable<Control> GetAllControlsRecursive(Control container)
        {
            if (container == null) yield break;

            foreach (Control control in container.Controls)
            {
                yield return control;

                foreach (Control childControl in GetAllControlsRecursive(control))
                {
                    yield return childControl;
                }
            }
        }

        public static void ApplyFontToContainer(Control container, string[] excludedControlNames = null, 
            float fontSizeMultiplier = 1.0f)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            if (_customFontFamily == null && !Initialize()) return;

            var excludedNames = excludedControlNames ?? Array.Empty<string>();
            var excludedSet = new HashSet<string>(excludedNames, StringComparer.OrdinalIgnoreCase);

            foreach (Control control in GetAllControlsRecursive(container))
            {
                if (!string.IsNullOrEmpty(control.Name) && excludedSet.Contains(control.Name)) continue;

                try
                {
                    float newSize = control.Font.Size * fontSizeMultiplier;
                    control.Font = CreateFont(newSize, control.Font.Style);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to apply font to control '{control.Name}': {ex.Message}");
                }
            }
        }

        public static void ApplyFontToControl(Control control, float? size = null, FontStyle? style = null)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));

            if (_customFontFamily == null && !Initialize()) return;

            try
            {
                float fontSize = size ?? control.Font.Size;
                FontStyle fontStyle = style ?? control.Font.Style;

                control.Font = CreateFont(fontSize, fontStyle);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to apply font to control '{control.Name}': {ex.Message}");
            }
        }

        public static void ReloadFont()
        {
            Cleanup();

            if (!string.IsNullOrEmpty(SettingsManager.Settings.LauncherFont))
            {
                Initialize();
            }
        }

        public static bool IsCustomFontAvailable()
        {
            return _customFontFamily != null || Initialize();
        }

        public static void Cleanup()
        {
            _fontCollection?.Dispose();
            _fontCollection = null;
            _customFontFamily = null;
        }
    }
}