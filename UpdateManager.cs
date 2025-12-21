using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STGCLauncher
{
    public class UpdateManager
    {
        private readonly HttpClient _httpClient;
        private readonly string _currentVersion;
        private readonly string _updateUrl;
        private readonly string _githubApiUrl;
        private readonly string _appPath;
        private readonly string _excludedFolders;
        private readonly string _updaterUrl;

        public UpdateManager(string githubRepo, string appPath, string excludedFolders = "Data;Fonts")
        {
            _httpClient = HttpClientFactory.Create();
            _currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _updateUrl = $"https://github.com/{githubRepo}/releases/latest/download/Launcher.zip";
            _githubApiUrl = $"https://api.github.com/repos/{githubRepo}/releases/latest";
            _appPath = appPath;
            _excludedFolders = excludedFolders;
            _updaterUrl = SettingsManager.Settings.UpdaterLink;
        }

        public async Task<bool> CheckForUpdatesAsync()
        {
            string latestVersionFromApi = null;

            try
            {
                try
                {
                    var apiResponse = await _httpClient.GetStringAsync(_githubApiUrl);
                    if (apiResponse.Contains("\"tag_name\""))
                    {
                        int start = apiResponse.IndexOf("\"tag_name\":\"") + 12;
                        int end = apiResponse.IndexOf("\"", start);

                        latestVersionFromApi = apiResponse.Substring(start, end - start);
                    }
                }
                catch
                {
                }

                if (string.IsNullOrEmpty(latestVersionFromApi)) return false;

                Version current = new Version(_currentVersion);
                Version latest = new Version(latestVersionFromApi);

                return latest > current;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DownloadAndApplyUpdateAsync()
        {
            try
            {
                string updaterPath = Path.Combine(Path.GetDirectoryName(_appPath), "Updater.exe");

                if (!File.Exists(updaterPath)) await DownloadUpdaterAsync(updaterPath);

                string args = $"\"{_appPath}\" \"{_updateUrl}\" \"{Path.GetDirectoryName(_appPath)}\" \"{_excludedFolders}\"";

                Process.Start(updaterPath, args);
                Application.Exit();

                return true;
            }
            catch
            {
                return false;
                throw;
            }
        }

        private async Task DownloadUpdaterAsync(string savePath)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(_updaterUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream
                    (
                         savePath,
                         FileMode.Create,
                         FileAccess.Write,
                         FileShare.None,
                         81920,
                         true
                    ))
                    {
                        await contentStream.CopyToAsync(fileStream, 81920);
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
