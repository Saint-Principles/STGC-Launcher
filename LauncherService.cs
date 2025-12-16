using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace STGCLauncher
{
    public static class HttpClientFactory
    {
        public static HttpClient Create()
        {
            var client = new HttpClient
            {
                Timeout = Timeout.InfiniteTimeSpan
            };

            client.DefaultRequestHeaders.ConnectionClose = false;
            client.DefaultRequestHeaders.UserAgent.ParseAdd(
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true,
                NoStore = true
            };

            return client;
        }
    }

    public static class NewsParser
    {
        public static NewsData Parse(string text, Image image)
        {
            var news = new NewsData { Image = image };

            if (string.IsNullOrEmpty(text))
            {
                news.Title = "News";
                news.Body = "No news available.";
                return news;
            }

            int newLineIndex = text.IndexOf('\n');

            if (newLineIndex <= 0)
            {
                news.Title = text;
                news.Body = string.Empty;
            }
            else
            {
                news.Title = text.Substring(0, newLineIndex).TrimEnd('\n');
                news.Body = text.Substring(newLineIndex + 1).Trim();
            }

            return news;
        }
    }

    public class LauncherService
    {
        private readonly HttpClient _httpClient;
        private readonly LauncherData _launcherData;

        public string LauncherUpdateLink => _launcherData.LauncherUpdateLink;
        public string GamePath => _launcherData.GamePath;
        public string FullGamePath => _launcherData.FullGamePath;
        public string CurrentVersion => _launcherData.CurrentVersion;
        public string LatestVersion => _launcherData.LatestVersion;
        public string TempArchivePath => _launcherData.TempArchivePath;
        public bool IsGameInstalled => _launcherData.IsGameInstalled;

        public LauncherService()
        {
            _launcherData = new LauncherData();
            _httpClient = HttpClientFactory.Create();
        }

        public LauncherService(string customGamePath, string gameName = null)
        {
            _launcherData = new LauncherData
            {
                GamePath = customGamePath
            };

            if (!string.IsNullOrEmpty(gameName))
            {
                _launcherData.GameName = gameName;
            }

            _httpClient = HttpClientFactory.Create();
        }

        public void LoadCurrentVersion()
        {
            _launcherData.LoadCurrentVersion();
        }

        public async Task<bool> CheckLatestVersionAsync()
        {
            _launcherData.LatestVersion = await DownloadTextAsync(_launcherData.LatestVersionFileLink);
            return !string.IsNullOrEmpty(_launcherData.LatestVersion);
        }

        public bool IsUpdateAvailable()
        {
            return !string.Equals(CurrentVersion, LatestVersion, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<NewsData> LoadNewsAsync()
        {
            var newsText = await DownloadTextAsync(_launcherData.NewsTextLink);
            var newsImage = await DownloadImageAsync(_launcherData.NewsImageLink);

            return NewsParser.Parse(newsText, newsImage);
        }

        public async Task<DownloadResult> DownloadGameAsync(IProgress<double> progress)
        {
            var result = await DownloadFileAsync(_launcherData.GameArchiveLink, TempArchivePath, progress);
            
            if (result.Success) await ExtractAndUpdateAsync(TempArchivePath);

            return result;
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
                    _launcherData.EnsureGameDirectoryExists();

                    using (var archive = ZipFile.OpenRead(archivePath))
                    {
                        var topLevelEntries = archive.Entries
                            .Select(e => e.FullName.Split('/')[0])
                            .Distinct()
                            .ToList();

                        bool hasSingleTopLevelFolder = topLevelEntries.Count == 1 &&
                                                      archive.Entries.Any(e => e.FullName.Contains("/"));

                        if (hasSingleTopLevelFolder)
                        {
                            string topFolder = topLevelEntries[0];

                            foreach (var entry in archive.Entries)
                            {
                                if (string.IsNullOrEmpty(entry.Name)) continue;

                                string relativePath = entry.FullName.Substring(topFolder.Length).TrimStart('/');

                                if (string.IsNullOrEmpty(relativePath)) continue;

                                string destinationPath = Path.Combine(_launcherData.FullGamePath, relativePath);
                                string destinationDir = Path.GetDirectoryName(destinationPath);

                                if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                                {
                                    Directory.CreateDirectory(destinationDir);
                                }

                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                        else
                        {
                            foreach (var entry in archive.Entries)
                            {
                                if (string.IsNullOrEmpty(entry.Name))
                                {
                                    string dirPath = Path.Combine(_launcherData.FullGamePath, entry.FullName);

                                    if (!Directory.Exists(dirPath))
                                    {
                                        Directory.CreateDirectory(dirPath);
                                    }

                                    continue;
                                }

                                string destinationPath = Path.Combine(_launcherData.FullGamePath, entry.FullName);
                                string destinationDir = Path.GetDirectoryName(destinationPath);

                                if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
                                {
                                    Directory.CreateDirectory(destinationDir);
                                }

                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                    }

                    File.WriteAllText(_launcherData.VersionFilePath, _launcherData.LatestVersion);
                    _launcherData.CurrentVersion = _launcherData.LatestVersion;

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