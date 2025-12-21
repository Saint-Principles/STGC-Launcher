using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace STGCLauncher
{
    public class LauncherConfiguration
    {
        [JsonIgnore] private const string DEFAULT_GAME_NAME = "Slendytubbies Guardian Collection";
        [JsonIgnore] private const string VERSION_FILE_NAME = "version";
        [JsonIgnore] private const string GAME_EXE_NAME = "Slendytubbies Guardian Collection.exe";

        [JsonIgnore] private string _gamePath;
        [JsonIgnore] private string _gameName;

        [JsonIgnore]
        public string CurrentVersion
        {
            get
            {
                if (IsGameInstalled)
                {
                    try
                    {
                        string versionFile = SettingsManager.Settings.VersionFilePath;
                        return File.ReadAllText(versionFile).Trim();
                    }
                    catch
                    {
                        return "0.0";
                    }
                }

                return "0.0";
            }
        }

        [JsonIgnore] public string LatestVersion { get; set; } = "0.0";

        [JsonProperty("gamePath")]
        public string GamePath
        {
            get => _gamePath ?? GetDefaultGamePath();
            set => _gamePath = value;
        }

        [JsonIgnore]
        public string GameName
        {
            get => _gameName ?? DEFAULT_GAME_NAME;
            set => _gameName = value;
        }

        [JsonProperty("launcherLanguage")] 
        public int LauncherLanguage { get; set; } = 0;
        [JsonProperty("fullScreenMode")] 
        public int FullScreenMode { get; set; } = 0;
        [JsonProperty("graphicsQuality")] 
        public int GraphicsQuality { get; set; } = 6;
        [JsonProperty("screenResolution")] 
        public int ScreenResolution { get; set; } = -1;
        [JsonProperty("mouseSensitivity")] 
        public int MouseSensitivity { get; set; } = 0;

        [JsonProperty("launcherFont")]
        public string LauncherFont { get; set; } = Path.Combine(Application.StartupPath, "Resources", "ldslender.ttf");
        [JsonProperty("gameArchiveLink")]
        public string GameArchiveLink { get; set; } = "https://drive.usercontent.google.com/download?id=1hz1v2xECytB1fPv9ydfk4-rmCwNvmTfr&export=download&authuser=0&confirm=t";
        [JsonProperty("latestVersionFileLink")]
        public string LatestVersionFileLink { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/version.txt";
        [JsonProperty("newsTextLink")]
        public string NewsTextLink { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news.txt";
        [JsonProperty("newsImageLink")] 
        public string NewsImageLink { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news.jpg";
        [JsonProperty("launcherUpdateLink")]
        public string LauncherUpdateLink { get; set; } = "https://github.com/Saint-Principles/STGC-Launcher/releases";
        [JsonProperty("newsTextLinkRus")]
        public string NewsTextLinkRus { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news_rus.txt";
        [JsonProperty("newsImageLinkRus")]
        public string NewsImageLinkRus { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news_rus.jpg";
        [JsonProperty("newsTextLinkEng")]
        public string NewsTextLinkEng { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news_eng.txt";
        [JsonProperty("newsImageLinkEng")]
        public string NewsImageLinkEng { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news_eng.jpg";
        [JsonProperty("updaterLink")]
        public string UpdaterLink { get; set; } = "https://github.com/Saint-Principles/Updater/releases/latest/download/Updater.exe";

        [JsonIgnore] 
        public string FullGamePath => Path.Combine(GamePath, GameName);
        [JsonIgnore] 
        public string VersionFilePath => Path.Combine(FullGamePath, VERSION_FILE_NAME);
        [JsonIgnore] 
        public string GameExecutablePath => Path.Combine(FullGamePath, GAME_EXE_NAME);
        [JsonIgnore]
        public string TempArchivePath => Path.Combine(Path.GetTempPath(), $"{GameName}.zip");

        [JsonIgnore]
        public bool IsGameInstalled
        {
            get
            {
                try
                {
                    string versionFile = SettingsManager.Settings.VersionFilePath;
                    string gameExe = SettingsManager.Settings.GameExecutablePath;
                    return File.Exists(versionFile) && File.Exists(gameExe);
                }
                catch
                {
                    return false;
                }
            }
        }

        private string GetDefaultGamePath()
        {
            string driveC = Environment.GetFolderPath(Environment.SpecialFolder.Windows).Substring(0, 3);
            string defaultPath = Path.Combine(driveC, "Games");

            try
            {
                if (!Directory.Exists(defaultPath))
                {
                    Directory.CreateDirectory(defaultPath);
                }
            }
            catch
            {
                defaultPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            }

            return defaultPath;
        }

        public string GetNewsTextLink()
        {
            switch (LocalizationManager.GetCurrentLanguage())
            {
                case "eng": return NewsTextLinkEng;
                case "rus": return NewsTextLinkRus;
                default: return NewsTextLink;
            }
        }

        public string GetNewsImageLink()
        {
            switch (LocalizationManager.GetCurrentLanguage())
            {
                case "eng": return NewsImageLinkEng;
                case "rus": return NewsImageLinkRus;
                default: return NewsImageLink;
            }
        }
    }
}