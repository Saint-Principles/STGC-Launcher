using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;

namespace STGCLauncher
{
    public class Fonts
    {
        public static PrivateFontCollection AddFontFromFiles(string font = "ldslender.ttf")
        {
            string fontPath = Path.Combine(Application.StartupPath, "Resources", font);

            if (File.Exists(fontPath))
            {
                var pfc = new PrivateFontCollection();
                pfc.AddFontFile(fontPath);

                return pfc;
            }

            return null;
        }

        public static IEnumerable<Control> GetAllControls(Control container)
        {
            foreach (Control control in container.Controls)
            {
                yield return control;

                foreach (Control child in GetAllControls(control)) yield return child;
            }
        }

        public static void ApplyCustomFontToAllControls(Control container, string[] excludedControls, string font = "ldslender.ttf")
        {
            var pfc = AddFontFromFiles(font);

            foreach (Control control in GetAllControls(container))
            {
                foreach (string controlName in excludedControls)
                {
                    if (control.Name == controlName) return;
                }

                control.Font = new Font(pfc.Families[0], control.Font.Size, control.Font.Style);
            }
        }
    }
}
