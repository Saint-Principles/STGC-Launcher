using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace STGCLauncher
{
    public class LauncherService
    {
        private readonly HttpClient _httpClient;

        public string LauncherUpdateLink => SettingsManager.Settings.LauncherUpdateLink;
        public string GamePath => SettingsManager.Settings.GamePath;
        public string FullGamePath => SettingsManager.Settings.FullGamePath;
        public string CurrentVersion => SettingsManager.Settings.CurrentVersion;
        public string LatestVersion => SettingsManager.Settings.LatestVersion;
        public string TempArchivePath => SettingsManager.Settings.TempArchivePath;
        public bool IsGameInstalled => SettingsManager.Settings.IsGameInstalled;

        public LauncherService()
        {
            _httpClient = HttpClientFactory.Create();
        }

        public bool IsUpdateAvailable()
        {
            if (string.IsNullOrEmpty(CurrentVersion) || string.IsNullOrEmpty(LatestVersion)) return false;

            try
            {
                var current = new Version(CurrentVersion);
                var latest = new Version(LatestVersion);
                return current < latest;
            }
            catch
            {
                return !string.Equals(CurrentVersion, LatestVersion, StringComparison.OrdinalIgnoreCase);
            }
        }

        public async Task<bool> CheckLatestVersionAsync()
        {
            try
            {
                string latestVersion = await DownloadTextAsync(SettingsManager.Settings.LatestVersionFileLink);

                if (string.IsNullOrWhiteSpace(latestVersion))
                {
                    return false;
                }

                SettingsManager.Settings.LatestVersion = latestVersion.Trim();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<NewsData> LoadNewsAsync()
        {
            string newsTextLink = SettingsManager.Settings.GetNewsTextLink();
            string newsImageLink = SettingsManager.Settings.GetNewsImageLink();

            var newsText = await DownloadTextAsync(newsTextLink);
            var newsImage = await DownloadImageAsync(newsImageLink);

            return NewsParser.Parse(newsText, newsImage);
        }

        public async Task<DownloadResult> DownloadGameAsync(IProgress<double> progress)
        {
            var downloadResult = await DownloadFileAsync(SettingsManager.Settings.GameArchiveLink, TempArchivePath, progress);

            if (downloadResult.Success) await ExtractAndUpdateAsync(TempArchivePath);

            return downloadResult;
        }

        private async Task<string> DownloadTextAsync(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.ParseAdd("text/plain,text/html;q=0.9,text/*;q=0.8");

                using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode) return null;

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return await Task.FromResult(string.Empty);
            }
        }

        private async Task<Image> DownloadImageAsync(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Accept.ParseAdd("image/webp,image/apng,image/*,*/*;q=0.8");

                using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var memoryStream = new MemoryStream();

                    await response.Content.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    return Image.FromStream(memoryStream);
                }
            }
            catch
            {
                return await Task.FromResult<Image>(null);
            }
        }

        private async Task<DownloadResult> DownloadFileAsync(string url, string filePath,
            IProgress<double> progress = null, CancellationToken cancellationToken = default)
        {
            var result = new DownloadResult();
            string tempFilePath = filePath + ".tmp";

            try
            {
                var directory = Path.GetDirectoryName(tempFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    response.EnsureSuccessStatusCode();

                    long? totalBytes = response.Content.Headers.ContentLength;
                    using (var contentStream = await response.Content.ReadAsStreamAsync())
                    using (var fileStream = new FileStream
                    (
                         filePath,
                         FileMode.Create,
                         FileAccess.Write,
                         FileShare.None, 
                         81920, 
                         true
                    ))
                    {
                        if (progress == null || !totalBytes.HasValue || totalBytes.Value == 0)
                        {
                            await contentStream.CopyToAsync(fileStream, 81920, cancellationToken);
                        }
                        else
                        {
                            await CopyToWithProgressAsync(
                                contentStream,
                                fileStream,
                                totalBytes.Value,
                                progress,
                                81920,
                                cancellationToken);
                        }
                    }
                }

                result.Success = true;
            }
            catch (OperationCanceledException ex)
            {
                result.Success = false;
                result.WasCancelled = true;
                result.Error = ex;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex;
            }

            return result;
        }

        private async Task CopyToWithProgressAsync(
            Stream source,
            Stream destination,
            long totalSize,
            IProgress<double> progress,
            int bufferSize,
            CancellationToken cancellationToken)
        {
            var buffer = new byte[bufferSize];
            long totalRead = 0;
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                totalRead += bytesRead;

                cancellationToken.ThrowIfCancellationRequested();

                if (progress != null)
                {
                    double percentage = (double)totalRead / totalSize;
                    progress.Report(percentage);
                }
            }

            progress?.Report(1.0);
        }

        private async Task ExtractAndUpdateAsync(string archivePath)
        {
            await Task.Run(() =>
            {
                try
                {
                    string fullGamePath = SettingsManager.Settings.FullGamePath;

                    if (!Directory.Exists(fullGamePath))
                    {
                        Directory.CreateDirectory(fullGamePath);
                    }

                    using (var archive = ZipFile.OpenRead(archivePath))
                    {
                        var entries = archive.Entries.Where(e => !string.IsNullOrEmpty(e.Name)).ToList();

                        if (entries.Count == 0) return;

                        string firstEntryPath = entries[0].FullName;
                        char separator = firstEntryPath.Contains('/') ? '/' : '\\';

                        string[] firstPathParts = firstEntryPath.Split(separator);
                        string potentialRootFolder = firstPathParts[0];

                        bool allEntriesHaveSameRoot = entries.All(e =>
                        {
                            string[] parts = e.FullName.Split(separator);
                            return parts.Length > 1 && parts[0] == potentialRootFolder;
                        });

                        foreach (var entry in entries)
                        {
                            string destinationPath;

                            if (allEntriesHaveSameRoot && entry.FullName.StartsWith(potentialRootFolder + separator))
                            {
                                string relativePath = entry.FullName.Substring(potentialRootFolder.Length).TrimStart(separator);

                                if (string.IsNullOrEmpty(relativePath)) continue;

                                destinationPath = Path.Combine(fullGamePath, relativePath);
                            }
                            else
                            {
                                destinationPath = Path.Combine(fullGamePath, entry.FullName);
                            }

                            string destinationDir = Path.GetDirectoryName(destinationPath);

                            if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                            {
                                Directory.CreateDirectory(destinationDir);
                            }

                            entry.ExtractToFile(destinationPath, true);
                        }
                    }

                    File.WriteAllText(SettingsManager.Settings.VersionFilePath, SettingsManager.Settings.LatestVersion);
                    File.Delete(archivePath);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to extract game archive", ex);
                }
            });
        }
    }
}