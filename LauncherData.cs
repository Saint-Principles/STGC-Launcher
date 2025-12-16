using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace STGCLauncher
{
    public enum LauncherStatus
    {
        CHECKING_VERSION,
        OFFLINE,
        OUTDATED,
        UPDATING,
        READY,
    }

    public class DownloadResult
    {
        public bool Success { get; set; }
        public Exception Error { get; set; }
        public bool WasCancelled { get; set; }
    }

    public class NewsData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Image Image { get; set; }
    }

    public class LauncherData
    {
        private const string DEFAULT_GAME_NAME = "Slendytubbies Guardian Collection";
        private const string VERSION_FILE_NAME = "version";
        private const string GAME_EXE_NAME = "Slendytubbies Guardian Collection.exe";

        private string _gamePath;
        private string _gameName;

        public string CurrentVersion { get; set; } = "0.0";
        public string LatestVersion { get; set; } = "0.0";

        public string GamePath
        {
            get => _gamePath ?? GetDefaultGamePath();
            set => _gamePath = value;
        }

        public string GameName
        {
            get => _gameName ?? DEFAULT_GAME_NAME;
            set => _gameName = value;
        }

        public string FullGamePath => Path.Combine(GamePath, GameName);
        public string VersionFilePath => Path.Combine(FullGamePath, VERSION_FILE_NAME);
        public string GameExecutablePath => Path.Combine(FullGamePath, GAME_EXE_NAME);
        public bool IsGameInstalled => File.Exists(VersionFilePath) && File.Exists(GameExecutablePath);
        public string TempArchivePath => Path.Combine(Path.GetTempPath(), $"{GameName}.zip");
        public string GameArchiveLink { get; set; } = "http://149.28.53.244:8080/Build.zip";
        public string LatestVersionFileLink { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/version.txt";
        public string NewsTextLink { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news.txt";
        public string NewsImageLink { get; set; } = "https://raw.githubusercontent.com/Saint-Principles/STGCLauncher_Data/refs/heads/main/news/news.jpg";
        public string LauncherUpdateLink { get; set; } = "";

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

        public void LoadCurrentVersion()
        {
            if (IsGameInstalled && File.Exists(VersionFilePath))
            {
                CurrentVersion = File.ReadAllText(VersionFilePath).Trim();
            }
        }

        public void EnsureGameDirectoryExists()
        {
            if (!Directory.Exists(FullGamePath))
            {
                Directory.CreateDirectory(FullGamePath);
            }
        }

        public string[] GetGameFiles()
        {
            if (!Directory.Exists(FullGamePath)) return Array.Empty<string>();

            return Directory.GetFiles(FullGamePath, "*", SearchOption.AllDirectories);
        }

        public string[] GetZipFiles(string zipPath)
        {
            if (!File.Exists(zipPath)) return Array.Empty<string>();

            using (var archive = ZipFile.OpenRead(zipPath))
            {
                return archive.Entries
                    .Where(e => !string.IsNullOrEmpty(e.Name))
                    .Select(e => e.FullName.Replace('/', Path.DirectorySeparatorChar))
                    .ToArray();
            }
        }
    }
}
