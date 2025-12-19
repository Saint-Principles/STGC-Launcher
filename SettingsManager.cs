using Newtonsoft.Json;
using System;
using System.IO;

namespace STGCLauncher
{
    public static class SettingsManager
    {
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.CurrentDirectory,
            "Data",
            "launcher_data.json"
        );

        private static LauncherConfiguration _settings;
        private static readonly object _lock = new object();

        static SettingsManager()
        {
            string settingsDir = Path.GetDirectoryName(SettingsFilePath);
            if (!Directory.Exists(settingsDir))
            {
                Directory.CreateDirectory(settingsDir);
            }
        }

        public static LauncherConfiguration Settings
        {
            get
            {
                if (_settings == null) LoadSettings();
                return _settings;
            }
        }

        #region Settings Properties

        public static string GamePath
        {
            get => Settings.GamePath;
            set => UpdateSetting(nameof(GamePath), value);
        }

        public static int LauncherLanguage
        {
            get => Settings.LauncherLanguage;
            set => UpdateSetting(nameof(LauncherLanguage), value);
        }

        public static int FullScreenMode
        {
            get => Settings.FullScreenMode;
            set => UpdateSetting(nameof(FullScreenMode), value);
        }

        public static int GraphicsQuality
        {
            get => Settings.GraphicsQuality;
            set => UpdateSetting(nameof(GraphicsQuality), value);
        }

        public static int ScreenResolution
        {
            get => Settings.ScreenResolution;
            set => UpdateSetting(nameof(ScreenResolution), value);
        }

        public static int MouseSensitivity
        {
            get => Settings.MouseSensitivity;
            set => UpdateSetting(nameof(MouseSensitivity), value);
        }

        #endregion

        #region Public Methods

        public static void LoadSettings()
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(SettingsFilePath))
                    {
                        string json = File.ReadAllText(SettingsFilePath);
                        _settings = JsonConvert.DeserializeObject<LauncherConfiguration>(json);
                    }
                    else
                    {
                        _settings = new LauncherConfiguration();
                        SaveSettings();
                    }
                }
                catch
                {
                    _settings = new LauncherConfiguration();
                }
            }
        }

        public static void SaveSettings()
        {
            lock (_lock)
            {
                try
                {
                    var settings = new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        DefaultValueHandling = DefaultValueHandling.Include
                    };

                    string json = JsonConvert.SerializeObject(_settings, settings);
                    WriteToFileWithBackup(json);
                }
                catch
                {
                    throw;
                }
            }
        }

        public static void ResetSettings()
        {
            lock (_lock)
            {
                try
                {
                    _settings = new LauncherConfiguration();
                    SaveSettings();
                }
                catch
                {
                    throw;
                }
            }
        }

        public static T GetValue<T>(string key, T defaultValue = default)
        {
            try
            {
                var property = typeof(LauncherConfiguration).GetProperty(key);
                if (property != null)
                {
                    var value = property.GetValue(Settings);
                    return value != null ? (T)value : defaultValue;
                }
            }
            catch
            {
                throw;
            }

            return defaultValue;
        }

        public static void SetValue<T>(string key, T value)
        {
            try
            {
                var property = typeof(LauncherConfiguration).GetProperty(key);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(Settings, value);
                }

                SaveSettings();
            }
            catch
            {
                throw;
            }
        }

        #endregion

        #region Private Helper Methods

        private static void UpdateSetting<T>(string propertyName, T value)
        {
            try
            {
                var property = typeof(LauncherConfiguration).GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    property.SetValue(Settings, value);
                }

                SaveSettings();
            }
            catch
            {
                throw;
            }
        }

        private static void WriteToFileWithBackup(string content)
        {
            string tempFile = SettingsFilePath + ".tmp";
            File.WriteAllText(tempFile, content);

            if (File.Exists(SettingsFilePath)) File.Delete(SettingsFilePath);

            File.Move(tempFile, SettingsFilePath);
        }

        #endregion
    }
}
