using Squirrel;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STGCLauncher
{
    public partial class Window : Form
    {
        private readonly LauncherService _launcherService;
        private LauncherStatus _launcherStatus;
        private Color _readyButtonColor = Color.FromArgb(179, 0, 0);
        private Color _notReadyButtonColor = Color.FromArgb(64, 64, 64);

        public Window()
        {
            InitializeComponent();
            SetupFormDrag();

            _launcherService = new LauncherService();
        }

        private async void window_Load(object sender, EventArgs e)
        {
            Fonts.ApplyCustomFontToAllControls(this, new string[]{ "exitButton" });

            await ApplyUpdatesIfAvailable();

            _launcherService.LoadCurrentVersion();
            UpdateVersionDisplay();

            ResetUIState();
            SetStatusText("Checking for updates");

            bool isOnline = await CheckForUpdatesAsync();

            if (!isOnline)
            {
                if (_launcherService.IsGameInstalled)
                {
                    SetLauncherStatus(LauncherStatus.OFFLINE);
                    UpdateStartButton("PLAY OFFLINE", true);
                    SetStatusText("Offline");
                }
                else
                {
                    SetLauncherStatus(LauncherStatus.CHECKING_VERSION);
                    UpdateStartButton("NOT DOWNLOADED", false);
                    SetStatusText("Offline - Connect to internet to download");
                }

                return;
            }

            await DetermineLauncherStateAsync();
            await LoadNewsAsync();
        }

        private async void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                switch (_launcherStatus)
                {
                    case LauncherStatus.READY:
                    case LauncherStatus.OFFLINE:
                        StartGame(); break;

                    case LauncherStatus.OUTDATED:
                        await DownloadUpdateAsync(); break;
                }
            }
            catch
            {
                throw;
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            // Открывать меню (форму) настроек
        }

        private void exitButton_Click(object sender, EventArgs e) => Application.Exit();

        private void SetupFormDrag()
        {
            MouseDown += new MouseEventHandler((sender, e) =>
            {
                Capture = false;

                Message msg = Message.Create(Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref msg);
            });
        }

        private void UpdateVersionDisplay()
        {
            versionLabel.Text = _launcherService.CurrentVersion == "0.0"
                ? "Not downloaded"
                : $"v{_launcherService.CurrentVersion}";
        }

        private void ResetUIState()
        {
            updateProgressLabel.Visible = false;
            startButton.BackColor = _notReadyButtonColor;
            startButton.Text = "...";
            startButton.Enabled = false;

            newsTitleLabel.Text = string.Empty;
            newsTextLabel.Text = string.Empty;
            newsTextLabel.Visible = false;
            newsImage.Image = null;
        }

        private void SetStatusText(string status)
        {
            statusLabel.Text = status;
        }

        private void SetStatusVisible(bool visible)
        {
            statusLabel.Visible = visible;
        }

        private void SetLauncherStatus(LauncherStatus status)
        {
            _launcherStatus = status;
        }

        private void UpdateStartButton(string text, bool enabled)
        {
            startButton.Text = text;
            startButton.Enabled = enabled;
            startButton.BackColor = enabled ? _readyButtonColor : _notReadyButtonColor;
        }

        private void StartGame()
        {
            //SaveUnityPrefs();
            bool isFullScreen = true;
            int selectedMonitorIndex = GetSelectedMonitorIndex();
            var (width, height) = GetSelectedResolution();

            Screen selectedScreen = GetValidScreen(selectedMonitorIndex);

            if (isFullScreen)
            {
                width = selectedScreen.Bounds.Width;
                height = selectedScreen.Bounds.Height;
            }

            string arguments = BuildLaunchArguments(
                width,
                height,
                isFullScreen,
                selectedMonitorIndex
            );

            string gameExePath = Path.Combine(
                _launcherService.FullGamePath,
                "Slendytubbies Guardian Collection.exe"
            );

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = gameExePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
            };

            Process gameProcess = new Process()
            {
                StartInfo = startInfo
            };

            gameProcess.Start();
            Application.Exit();
        }

        private int GetSelectedMonitorIndex()
        {
            //if (cmbMonitor.SelectedIndex < 0) return 0;

            int availableMonitors = Screen.AllScreens.Length;

            //if (cmbMonitor.SelectedIndex >= availableMonitors)
            //{
            //    return Math.Max(0, availableMonitors - 1);
            //}

            //return cmbMonitor.SelectedIndex;

            return 0;
        }

        private (int Width, int Height) GetSelectedResolution()
        {
            const string DEFAULT_RESOLUTION = "1920x1080";
            const char RESOLUTION_SEPARATOR = 'x';

            //string selectedResolution = cmbResolution?.SelectedItem as string;
            string selectedResolution = "";

            if (string.IsNullOrWhiteSpace(selectedResolution))
            {
                selectedResolution = DEFAULT_RESOLUTION;
            }

            string normalizedResolution = selectedResolution
                .Replace(" ", "")
                .Replace("×", "x")
                .Replace("*", "x")
                .ToLower(); 

            string[] resolutionParts = normalizedResolution.Split(RESOLUTION_SEPARATOR);

            int.TryParse(resolutionParts[0], out int width);
            int.TryParse(resolutionParts[1], out int height);

            return (width, height);
        }

        private Screen GetValidScreen(int monitorIndex)
        {
            int safeIndex = Clamp(monitorIndex, 0, Screen.AllScreens.Length - 1);

            return Screen.AllScreens[safeIndex];
        }

        private int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        private string BuildLaunchArguments(int width, int height, bool isFullScreen, int monitorIndex)
        {
            var args = new StringBuilder();

            if (isFullScreen) args.Append("-screen-fullscreen 1 ");
            if (!isFullScreen) args.Append("-popupwindow ");

            args.Append($"-screen-width {width} ");
            args.Append($"-screen-height {height} ");
            args.Append("-screen-fullscreen 0 ");
            args.Append($"-monitor {monitorIndex + 1}");

            return args.ToString();
        }


        private async Task ApplyUpdatesIfAvailable()
        {
            try
            {
                using (var updateManager = await UpdateManager.GitHubUpdateManager(_launcherService.LauncherUpdateLink))
                {
                    if (updateManager == null) return;

                    var updateInfo = await updateManager.CheckForUpdate();

                    if (updateInfo.ReleasesToApply.Count > 0)
                    {
                        updatePanel.BringToFront();
                        updatePanel.Visible = true;

                        await updateManager.DownloadReleases(updateInfo.ReleasesToApply);
                        await updateManager.ApplyReleases(updateInfo);

                        UpdateManager.RestartApp();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                return await _launcherService.CheckLatestVersionAsync();
            }
            catch
            {
                return false;
            }
        }

        private async Task DetermineLauncherStateAsync()
        {
            bool isUpdateAvailable = _launcherService.IsUpdateAvailable();

            if (isUpdateAvailable)
            {
                SetLauncherStatus(LauncherStatus.OUTDATED);

                string buttonText = _launcherService.IsGameInstalled ? "UPDATE NOW" : "DOWNLOAD NOW";
                string statusText = _launcherService.IsGameInstalled
                    ? $"A new update is available! (v{_launcherService.LatestVersion})"
                    : $"Ready to download latest version! (v{_launcherService.LatestVersion})";

                UpdateStartButton(buttonText, true);
                SetStatusText(statusText);
            }
            else
            {
                SetLauncherStatus(LauncherStatus.READY);
                UpdateStartButton("PLAY", true);
                SetStatusText("Ready to play!");
            }
        }

        private async Task LoadNewsAsync()
        {
            try
            {
                var news = await _launcherService.LoadNewsAsync();
                UpdateNews(news);
            }
            catch
            {
                ShowDefaultNews();
            }
        }

        private async Task DownloadUpdateAsync()
        {
            try
            {
                PrepareForDownload();

                var progress = new Progress<double>(UpdateDownloadProgress);
                var result = await _launcherService.DownloadGameAsync(progress);

                HandleDownloadResult(result);
            }
            catch
            {
                HandleDownloadError();
            }
        }

        private void PrepareForDownload()
        {
            SetLauncherStatus(LauncherStatus.UPDATING);

            string buttonText = _launcherService.IsGameInstalled ? "DOWNLOADING UPDATE" : "DOWNLOADING";
            string progressText = _launcherService.IsGameInstalled ? "Update Progress" : "Download Progress";

            UpdateStartButton(buttonText, false);
            SetStatusVisible(false);

            updateProgressLabel.Text = progressText;
            updateProgressLabel.Visible = true;
            progressBarBackground.Visible = true;

            ResetProgressBar();
        }

        private void UpdateDownloadProgress(double percentage)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<double>(UpdateDownloadProgress), percentage);
                return;
            }

            int width = (int)(percentage * 404);
            progressBar.Width = width;
            progressBar.Text = $"{(percentage * 100):F0}%";
        }

        private void HandleDownloadResult(DownloadResult result)
        {
            if (result.Success)
            {
                SetLauncherStatus(LauncherStatus.READY);
                ShowDownloadComplete();
                UpdateVersionDisplay();
            }
            else
            {
                HandleDownloadError();
            }
        }

        private void HandleDownloadError()
        {
            SetLauncherStatus(LauncherStatus.OUTDATED);

            progressBarBackground.Visible = false;
            updateProgressLabel.Visible = false;

            UpdateStartButton("DOWNLOAD AGAIN", true);
            SetStatusVisible(true);
            SetStatusText("Error! Try again later...");
        }

        private void ShowDownloadComplete()
        {
            progressBarBackground.Visible = false;
            updateProgressLabel.Visible = false;

            UpdateStartButton("PLAY", true);
            SetStatusVisible(true);
            SetStatusText("Ready to play!");
        }

        private void UpdateNews(NewsData news)
        {
            newsTitleLabel.Text = news.Title;
            newsTextLabel.Text = news.Body;
            newsTextLabel.Visible = !string.IsNullOrEmpty(news.Body);
            newsImage.Image = news.Image;
        }

        private void ShowDefaultNews()
        {
            newsTitleLabel.Text = "News";
            newsTextLabel.Text = "No news available.";
            newsTextLabel.Visible = true;
            newsImage.Image = null;
        }

        private void ResetProgressBar()
        {
            progressBar.Text = "0%";
            progressBar.Width = 0;
        }
    }
}
