using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STGCLauncher
{
    public partial class MainWindow : Form
    {
        private readonly LauncherService _launcherService;
        private readonly UpdateManager _updateManager;
        private SettingsWindow _settingsWindow;
        private LauncherStatus _launcherStatus;

        private readonly Color _readyButtonColor = Color.FromArgb(179, 0, 0);
        private readonly Color _notReadyButtonColor = Color.FromArgb(64, 64, 64);

        private readonly string[] _excludedControlNames =
        {
            "exitButton", 
            "progressBar",
        };

        public MainWindow()
        {
            InitializeComponent();
            SetupFormDrag();
            FontManager.Initialize();

            _launcherService = new LauncherService();

            _settingsWindow = new SettingsWindow();
            _settingsWindow.Hide();
            _settingsWindow.OnCustomFolderChanged = delegate
            {
                _ = InitializeLauncherAsync();
            };

            string appPath = Assembly.GetExecutingAssembly().Location;
            _updateManager = new UpdateManager("Saint-Principles/STGC-Launcher", appPath, "Data;Localizations;Fonts");

            int languageIndex = SettingsManager.LauncherLanguage;
            if (languageIndex >= 0 && languageIndex < LocalizationManager.AvailableLanguages.Length)
            {
                LocalizationManager.LoadLanguage(LocalizationManager.AvailableLanguages[languageIndex]);
            }

            LocalizationManager.LanguageChanged += OnLanguageChanged;
        }

        #region Event Handlers

        private async void Window_Load(object sender, EventArgs e)
        {
            ApplyLocalization();
            await InitializeLauncherAsync();
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                await HandleStartButtonClickAsync();
            }
            catch
            {
                HandleError("Failed to process action");
            }
        }

        private void SettingsButton_Click(object sender, EventArgs e) => ShowSettingsWindow();

        private void ExitButton_Click(object sender, EventArgs e)
        {
            LocalizationManager.LanguageChanged -= OnLanguageChanged;
            FontManager.Cleanup();
            Application.Exit();
        }

        #endregion

        #region Initialization Methods

        private async Task InitializeLauncherAsync()
        {
            await CheckForLauncherUpdatesAsync();

            UpdateVersionDisplay();

            ResetUIState();
            SetStatusText(LocalizationManager.GetString("CHECKING_FOR_UPDATES"));

            bool isOnline = await CheckForGameUpdatesAsync();

            if (!isOnline)
            {
                HandleOfflineState();
                return;
            }

            await DetermineLauncherStateAsync();
            await LoadNewsAsync();
        }

        private void HandleOfflineState()
        {
            if (_launcherService.IsGameInstalled)
            {
                SetLauncherStatus(LauncherStatus.OFFLINE);
                UpdateStartButton(LocalizationManager.GetString("PLAY_OFFLINE"), true);
                SetStatusText(LocalizationManager.GetString("OFFLINE"));
            }
            else
            {
                SetLauncherStatus(LauncherStatus.CHECKING_VERSION);
                UpdateStartButton(LocalizationManager.GetString("NOT_DOWNLOADED_BUTTON"), false);
                SetStatusText(LocalizationManager.GetString("OFFLINE_CONNECT_TO_INTERNET"));
            }
        }

        #endregion

        #region Button Action Handling

        private async Task HandleStartButtonClickAsync()
        {
            switch (_launcherStatus)
            {
                case LauncherStatus.READY:
                case LauncherStatus.OFFLINE:
                    StartGame(); 
                    break;


                case LauncherStatus.OUTDATED:
                    await DownloadUpdateAsync(); 
                    break;

                default: break;
            }
        }

        #endregion

        #region Form Setup

        private void SetupFormDrag()
        {
            MouseDown += (sender, e) =>
            {
                Capture = false;
                Message msg = Message.Create(Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref msg);
            };
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnLanguageChanged(sender, e)));
                return;
            }

            ApplyLocalization();
            _ = RefreshUIStateAsync();
            _ = LoadNewsAsync();
        }

        private async Task RefreshUIStateAsync()
        {
            string buttonText = "";
            string statusText = "";
            string progressText = "";

            bool isButtonEnabled = true;

            switch (_launcherStatus)
            {
                case LauncherStatus.READY:
                    buttonText = LocalizationManager.GetString("PLAY");
                    statusText = LocalizationManager.GetString("READY_TO_PLAY");
                    break;
                case LauncherStatus.OFFLINE:
                    buttonText = LocalizationManager.GetString("PLAY_OFFLINE");
                    statusText = LocalizationManager.GetString("OFFLINE");
                    break;
                case LauncherStatus.CHECKING_VERSION:
                    isButtonEnabled = false;
                    buttonText = LocalizationManager.GetString("NOT_DOWNLOADED_BUTTON");
                    statusText = LocalizationManager.GetString("OFFLINE_CONNECT_TO_INTERNET");
                    break;
                case LauncherStatus.UPDATING:
                    isButtonEnabled = false;
                    buttonText = _launcherService.IsGameInstalled
                        ? LocalizationManager.GetString("DOWNLOADING_UPDATE")
                        : LocalizationManager.GetString("DOWNLOADING");

                    progressText = _launcherService.IsGameInstalled
                        ? LocalizationManager.GetString("UPDATE_PROGRESS")
                        : LocalizationManager.GetString("DOWNLOAD_PROGRESS");

                    break;
                case LauncherStatus.OUTDATED:
                    buttonText = _launcherService.IsGameInstalled
                        ? LocalizationManager.GetString("UPDATE_NOW")
                        : LocalizationManager.GetString("DOWNLOAD_NOW");

                    statusText = _launcherService.IsGameInstalled
                        ? $"{LocalizationManager.GetString("NEW_UPDATE_AVAILABLE")} (v{_launcherService.LatestVersion})"
                        : $"{LocalizationManager.GetString("READY_TO_DOWNLOAD")} (v{_launcherService.LatestVersion})";
                    break;
            }

            updateProgressLabel.Text = progressText;

            UpdateStartButton(buttonText, isButtonEnabled);
            SetStatusText(statusText);
            UpdateVersionDisplay();
        }

        private void ApplyLocalization() => LocalizationManager.ApplyLocalizationToForm(this, _excludedControlNames);

        private void ShowSettingsWindow()
        {
            if (_settingsWindow == null || _settingsWindow.IsDisposed)
            {
                _settingsWindow = new SettingsWindow();
            }

            _settingsWindow.Show();
        }

        #endregion

        #region UI State Management

        private void ResetUIState()
        {
            updateProgressLabel.Visible = false;
            startButton.BackColor = _notReadyButtonColor;
            startButton.Text = "...";
            startButton.Enabled = false;

            ClearNewsDisplay();
        }

        private void ClearNewsDisplay()
        {
            newsTitleLabel.Text = string.Empty;
            newsTextLabel.Text = string.Empty;
            newsTextLabel.Visible = false;
            newsImage.Image = null;
        }

        private void UpdateVersionDisplay()
        {
            versionLabel.Text = _launcherService.CurrentVersion == "0.0"
                ? LocalizationManager.GetString("NOT_DOWNLOADED_VERSION")
                : $"v{_launcherService.CurrentVersion}";
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

        #endregion

        #region Game Launch Methods

        private void StartGame()
        {
            try
            {
                SaveUnityPreferences();
                var launchParameters = GetGameLaunchParameters();
                string gamePath = GetGameExecutablePath();

                if (!File.Exists(gamePath))
                {
                    ShowErrorMessage(LocalizationManager.GetString("GAME_EXECUTABLE_NOT_FOUND"));
                    return;
                }

                LaunchGameProcess(gamePath, launchParameters);
                Application.Exit();
            }
            catch
            {
                HandleError("Failed to start game");
            }
        }

        private void SaveUnityPreferences()
        {
            const string COMPANY_NAME = "ZeoWorks";
            const string PRODUCT_NAME = "Slendytubbies Guardian Collection";

            UnityPrefsRegistry.SetInt(COMPANY_NAME, PRODUCT_NAME, 
                "UnityGraphicsQuality", SettingsManager.GraphicsQuality);

            UnityPrefsRegistry.SetInt(COMPANY_NAME, PRODUCT_NAME, 
                "MouseSensitivity", SettingsManager.MouseSensitivity);
        }

        private LaunchParameters GetGameLaunchParameters()
        {
            int fullScreenMode = SettingsManager.FullScreenMode;
            int selectedMonitorIndex = 0;

            var (width, height) = GetSelectedResolution();
            Screen selectedScreen = GetValidScreen(selectedMonitorIndex);

            if (fullScreenMode == 0 && fullScreenMode == 2)
            {
                width = selectedScreen.Bounds.Width;
                height = selectedScreen.Bounds.Height;
            }

            return new LaunchParameters
            {
                Width = width,
                Height = height,
                FullScreenMode = fullScreenMode,
                MonitorIndex = selectedMonitorIndex
            };
        }

        private (int Width, int Height) GetSelectedResolution()
        {
            string selectedResolution = _settingsWindow?.SelectedResolution;
            string normalizedResolution = NormalizeResolutionString(selectedResolution);

            return ParseResolution(normalizedResolution);
        }

        private string NormalizeResolutionString(string resolution)
        {
            return resolution
                .Replace(" ", "")
                .Replace("×", "x")
                .Replace("*", "x")
                .ToLower();
        }

        private (int Width, int Height) ParseResolution(string resolution)
        {
            string[] parts = resolution.Split('x');

            int.TryParse(parts[0], out int width);
            int.TryParse(parts[1], out int height);

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

        private string GetGameExecutablePath()
        {
            return Path.Combine(_launcherService.FullGamePath, "Slendytubbies Guardian Collection.exe");
        }

        private void LaunchGameProcess(string gamePath, LaunchParameters parameters)
        {
            string arguments = BuildLaunchArguments(parameters);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = gamePath,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = false,
                CreateNoWindow = false,
            };

            using (Process gameProcess = new Process { StartInfo = startInfo })
            {
                gameProcess.Start();
            }
        }

        private string BuildLaunchArguments(LaunchParameters parameters)
        {
            var args = new StringBuilder();

            switch (parameters.FullScreenMode)
            {
                case 0:
                    args.Append("-screen-fullscreen 1 ");
                    args.Append("-fullscreen ");
                    break;
                case 1:
                    args.Append("-screen-fullscreen 0 ");
                    args.Append("-windowed ");
                    break;
                case 2:
                    args.Append("-screen-fullscreen 1 ");
                    args.Append("-borderless ");
                    break;
            }

            args.Append($"-screen-width {parameters.Width} ");
            args.Append($"-screen-height {parameters.Height} ");
            args.Append($"-screen-monitor {parameters.MonitorIndex + 1}");

            return args.ToString();
        }

        #endregion

        #region Update Methods

        private async Task CheckForLauncherUpdatesAsync()
        {
            try
            {
                if (await _updateManager.CheckForUpdatesAsync())
                {
                    ShowUpdatePanel();
                    await _updateManager.DownloadAndApplyUpdateAsync();
                }
            }
            catch
            {
                HandleError("Failed to check for launcher updates");
            }
        }

        private void ShowUpdatePanel()
        {
            updatePanel.BringToFront();
            updatePanel.Visible = true;
        }

        private async Task<bool> CheckForGameUpdatesAsync()
        {
            try
            {
                return await _launcherService.CheckLatestVersionAsync();
            }
            catch
            {
                HandleError("Failed to check for game updates");
                return false;
            }
        }

        private async Task DetermineLauncherStateAsync()
        {
            bool isUpdateAvailable = _launcherService.IsUpdateAvailable();

            if (isUpdateAvailable) HandleUpdateAvailableState();
            else HandleReadyState();
        }

        private void HandleUpdateAvailableState()
        {
            SetLauncherStatus(LauncherStatus.OUTDATED);

            string buttonText = _launcherService.IsGameInstalled 
                ? LocalizationManager.GetString("UPDATE_NOW") 
                : LocalizationManager.GetString("DOWNLOAD_NOW");

            string statusText = _launcherService.IsGameInstalled
                ? $"{LocalizationManager.GetString("NEW_UPDATE_AVAILABLE")} (v{_launcherService.LatestVersion})"
                : $"{LocalizationManager.GetString("READY_TO_DOWNLOAD")} (v{_launcherService.LatestVersion})";

            UpdateStartButton(buttonText, true);
            SetStatusText(statusText);
        }

        private void HandleReadyState()
        {
            SetLauncherStatus(LauncherStatus.READY);
            UpdateStartButton(LocalizationManager.GetString("PLAY"), true);
            SetStatusText(LocalizationManager.GetString("READY_TO_PLAY"));
        }

        #endregion

        #region Download Methods

        private async Task DownloadUpdateAsync()
        {
            try
            {
                PrepareForDownload();

                var progress = new Progress<double>(UpdateDownloadProgress);
                var result = await _launcherService.DownloadGameAsync(progress);

                HandleDownloadResult(result);
            }
            catch (Exception ex)
            {
                HandleDownloadError("Download failed", ex);
            }
        }

        private void PrepareForDownload()
        {
            SetLauncherStatus(LauncherStatus.UPDATING);

            string buttonText = _launcherService.IsGameInstalled 
                ? LocalizationManager.GetString("DOWNLOADING_UPDATE") 
                : LocalizationManager.GetString("DOWNLOADING");
            string progressText = _launcherService.IsGameInstalled 
                ? LocalizationManager.GetString("UPDATE_PROGRESS") 
                : LocalizationManager.GetString("DOWNLOAD_PROGRESS");

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
            progressBar.Width = Clamp(width, 0, 404);
            progressBar.Text = $"{percentage * 100:F0}%";
        }

        private void HandleDownloadResult(DownloadResult result)
        {
            if (result.Success)
            {
                SetLauncherStatus(LauncherStatus.READY);
                ShowDownloadComplete();
                UpdateVersionDisplay();
            }
            else if (result.WasCancelled)
            {
                HandleDownloadError("Download cancelled", null);
            }
            else
            {
                HandleDownloadError("Download failed", result.Error);
            }
        }

        private void ShowDownloadComplete()
        {
            progressBarBackground.Visible = false;
            updateProgressLabel.Visible = false;

            UpdateStartButton(LocalizationManager.GetString("PLAY"), true);
            SetStatusVisible(true);
            SetStatusText(LocalizationManager.GetString("READY_TO_PLAY"));
        }

        private void HandleDownloadError(string message, Exception ex)
        {
            SetLauncherStatus(LauncherStatus.OUTDATED);

            progressBarBackground.Visible = false;
            updateProgressLabel.Visible = false;

            UpdateStartButton(LocalizationManager.GetString("DOWNLOAD_AGAIN"), true);
            SetStatusVisible(true);
            SetStatusText(LocalizationManager.GetString("ERROR_TRY_AGAIN"));

            if (ex != null) HandleError(message);
        }

        private void ResetProgressBar()
        {
            progressBar.Text = "0%";
            progressBar.Width = 0;
        }

        #endregion

        #region News Methods

        private async Task LoadNewsAsync()
        {
            try
            {
                var news = await _launcherService.LoadNewsAsync();
                UpdateNews(news);
            }
            catch
            {
                HandleError("Failed to load news");
                ShowDefaultNews();
            }
        }

        private void UpdateNews(NewsData news)
        {
            if (news == null)
            {
                ShowDefaultNews();
                return;
            }

            newsTitleLabel.Text = news.Title;
            newsTextLabel.Text = news.Body;
            newsTextLabel.Visible = !string.IsNullOrEmpty(news.Body);
            newsImage.Image = news.Image;
        }

        private void ShowDefaultNews()
        {
            newsTitleLabel.Text = LocalizationManager.GetString("DEFAULT_NEWS_TITLE");
            newsTextLabel.Text = LocalizationManager.GetString("DEFAULT_NEWS_TEXT");
            newsTextLabel.Visible = true;
            newsImage.Image = null;
        }

        #endregion

        #region Error Handling

        private void HandleError(string message)
        {
            MessageBox.Show($"{message}. Please try again later.", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowErrorMessage(string message) => SetStatusText(message);

        #endregion
    }

    #region Supporting Classes

    internal class LaunchParameters
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int FullScreenMode { get; set; }
        public int MonitorIndex { get; set; }
    }

    #endregion
}