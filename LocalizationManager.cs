using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace STGCLauncher
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> _translations = new Dictionary<string, string>();
        private static Dictionary<string, int> _controlFontSizes = new Dictionary<string, int>();
        private static string _currentLanguage = "eng";
        private static string _currentFontPath;

        public static string CurrentFontPath => _currentFontPath;
        public static string[] AvailableLanguages { get; private set; } = Array.Empty<string>();
        public static event EventHandler LanguageChanged;

        static LocalizationManager()
        {
            LoadAvailableLanguages();
            LoadLanguage(_currentLanguage);
        }

        public static void LoadAvailableLanguages()
        {
            string localizationDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localizations");

            if (!Directory.Exists(localizationDir))
            {
                Directory.CreateDirectory(localizationDir);
                CreateDefaultTranslationFiles(localizationDir);
            }

            var files = Directory.GetFiles(localizationDir, "*.txt");
            AvailableLanguages = files.Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();

            if (!AvailableLanguages.Contains("eng"))
            {
                CreateDefaultTranslationFiles(localizationDir);
                AvailableLanguages = Directory.GetFiles(localizationDir, "*.txt")
                    .Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
            }
        }

        private static void CreateDefaultTranslationFiles(string dir)
        {
            string englishContent = @"Language: English
Font: Fonts\ldslender.ttf

// Control translations (with optional font size)
// MainWindow
""startButton"": ""UPDATE NOW"", fontSize: 18
""statusLabel"": ""A new update is available!"", fontSize: 16
""updateProgressLabel"": ""Update Progress"", fontSize: 11
""versionLabel"": ""Not downloaded"", fontSize: 12
""newsTitleLabel"": ""News"", fontSize: 20
""newsTextLabel"": ""No news available."", fontSize: 16

// MainWindow.UpdatingLauncher
""updateLabel1"": ""Launcher is updating..."", fontSize: 22
""updateLabel2"": ""An important update is downloading. Please do not close the launcher. It will automatically restart in a few seconds."", fontSize: 16

// SettingsWindow
""languageLabel"": ""Launcher Language:"", fontSize: 16
""fullscreenLabel"": ""Fullscreen Mode:"", fontSize: 16
""resolutionLabel"": ""Screen Resolution:"", fontSize: 16
""graphicsLabel"": ""Graphics Quality:"", fontSize: 16
""sensitivityLabel"": ""Mouse Sensitivity:"", fontSize: 16
""customFolderLabel"": ""Custom Folder:"", fontSize: 16
""saveButton"": ""SAVE"", fontSize: 18
""resetButton"": ""RESET"", fontSize: 18

// Text translations (without fontSize)
""PLAY_OFFLINE"": ""PLAY OFFLINE""
""PLAY"": ""PLAY""
""UPDATE_NOW"": ""UPDATE NOW""
""DOWNLOAD_NOW"": ""DOWNLOAD NOW""
""DOWNLOADING_UPDATE"": ""DOWNLOADING UPDATE""
""DOWNLOADING"": ""DOWNLOADING""
""DOWNLOAD_AGAIN"": ""DOWNLOAD AGAIN""
""NOT_DOWNLOADED"": ""NOT DOWNLOADED""
""NOT_DOWNLOADED_BUTTON"": ""NOT DOWNLOADED""
""NOT_DOWNLOADED_VERSION"": ""Not downloaded""
""CHECKING_FOR_UPDATES"": ""Checking for updates""
""OFFLINE"": ""Offline""
""OFFLINE_CONNECT_TO_INTERNET"": ""Offline - Connect to internet to download""
""NEW_UPDATE_AVAILABLE"": ""A new update is available!""
""READY_TO_DOWNLOAD"": ""Ready to download latest version!""
""READY_TO_PLAY"": ""Ready to play!""
""DOWNLOAD_PROGRESS"": ""Download Progress""
""UPDATE_PROGRESS"": ""Update Progress""
""ERROR_TRY_AGAIN"": ""Error! Try again later...""
""DEFAULT_NEWS_TITLE"": ""News""
""DEFAULT_NEWS_TEXT"": ""No news available.""
""GAME_EXECUTABLE_NOT_FOUND"": ""Game executable not found!""";

            File.WriteAllText(Path.Combine(dir, "eng.txt"), englishContent);
        }

        public static bool LoadLanguage(string languageCode)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Localizations", $"{languageCode}.txt");

            if (!File.Exists(filePath))
            {
                if (languageCode != "eng") return LoadLanguage("eng");

                return false;
            }

            _translations.Clear();
            _controlFontSizes.Clear();

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim();

                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("//"))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("Language:"))
                {
                    _currentLanguage = languageCode;
                    continue;
                }

                if (trimmedLine.StartsWith("Font:"))
                {
                    _currentFontPath = trimmedLine.Substring(5).Trim();
                    continue;
                }

                var fontSizeMatch = Regex.Match(trimmedLine, @",\s*fontSize:\s*(\d+)", RegexOptions.IgnoreCase);
                if (fontSizeMatch.Success)
                {
                    var keyMatch = Regex.Match(trimmedLine, @"""([^""]+)""\s*:\s*""([^""]+)""");
                    if (keyMatch.Success && keyMatch.Groups.Count >= 3)
                    {
                        string key = keyMatch.Groups[1].Value;
                        string value = keyMatch.Groups[2].Value;
                        int fontSize = int.Parse(fontSizeMatch.Groups[1].Value);

                        _translations[key] = value;
                        _controlFontSizes[key] = fontSize;
                    }
                }
                else
                {
                    var match = Regex.Match(trimmedLine, @"""([^""]+)""\s*:\s*""([^""]+)""");
                    if (match.Success && match.Groups.Count >= 3)
                    {
                        string key = match.Groups[1].Value;
                        string value = match.Groups[2].Value;
                        _translations[key] = value;
                    }
                }
            }

            if (!string.IsNullOrEmpty(_currentFontPath))
            {
                SettingsManager.Settings.LauncherFont = _currentFontPath;
                SettingsManager.SaveSettings();
                FontManager.ReloadFont();
            }

            LanguageChanged?.Invoke(null, EventArgs.Empty);
            return true;
        }

        public static string GetString(string key)
        {
            if (_translations.TryGetValue(key, out string value)) return value;

            return key;
        }

        public static bool HasControlTranslation(string controlName)
        {
            return _translations.ContainsKey(controlName);
        }

        public static int GetControlFontSize(string controlName)
        {
            if (_controlFontSizes.TryGetValue(controlName, out int fontSize))
            {
                return fontSize;
            }

            return 9;
        }

        public static string GetCurrentLanguage() => _currentLanguage;

        public static string GetLanguageDisplayName(string languageCode)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Localizations", $"{languageCode}.txt");

            if (!File.Exists(filePath)) return languageCode;

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("Language:")) return line.Substring(9).Trim();
            }

            return languageCode;
        }

        // Под вопросом
        public static void ApplyLocalizationToForm(Form form, string[] excludedControlNames = null)
        {
            if (form == null) return;

            foreach (Control control in GetAllControls(form))
            {
                if (!string.IsNullOrEmpty(control.Name) && HasControlTranslation(control.Name))
                {
                    control.Text = GetString(control.Name);

                    int fontSize = GetControlFontSize(control.Name);
                    if (fontSize > 0 && FontManager.IsCustomFontAvailable())
                    {
                        FontManager.ApplyFontToControl(control, fontSize);
                    }
                }
            }

            FontManager.ApplyFontToContainer(form, excludedControlNames);
        }

        private static IEnumerable<Control> GetAllControls(Control control)
        {
            var controls = new List<Control> { control };

            foreach (Control child in control.Controls)
            {
                controls.AddRange(GetAllControls(child));
            }

            return controls;
        }
    }
}