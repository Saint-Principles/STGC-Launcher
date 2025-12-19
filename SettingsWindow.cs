using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace STGCLauncher
{
    public partial class SettingsWindow : Form
    {
        public string SelectedResolution => resolutionComboBox.SelectedItem as string;
        public Action OnCustomFolderChanged = null;
        private string[] _excludedControlNames =
        {
            "exitButton",
            "customFolderButton",
            "languageComboBox",
            "fullscreenComboBox",
            "resolutionComboBox",
            "graphicsComboBox",
            "customFolderTextBox",
        };

        public SettingsWindow()
        {
            InitializeComponent();
            SetupFormDrag();
            InitializeSettings();
            InitializeLanguageComboBox();
            ApplyLocalization();

            FontManager.Initialize();
            FontManager.ApplyFontToContainer(this, _excludedControlNames);
        }

        private void InitializeLanguageComboBox()
        {
            languageComboBox.Items.Clear();
            foreach (var langCode in LocalizationManager.AvailableLanguages)
            {
                string displayName = LocalizationManager.GetLanguageDisplayName(langCode);
                languageComboBox.Items.Add(displayName);

                if (langCode == LocalizationManager.GetCurrentLanguage())
                {
                    languageComboBox.SelectedIndex = languageComboBox.Items.Count - 1;
                }
            }

            if (languageComboBox.SelectedIndex == -1 && languageComboBox.Items.Count > 0)
            {
                languageComboBox.SelectedIndex = 0;
            }
        }

        private void ApplyLocalization() => LocalizationManager.ApplyLocalizationToForm(this, _excludedControlNames);

        private void ExitButton_Click(object sender, EventArgs e) => Hide();

        private void SaveButton_Click(object sender, EventArgs e) => SettingsManager.SaveSettings();

        private void ResetButton_Click(object sender, EventArgs e)
        {
            SettingsManager.ResetSettings();
            InitializeSettings();
        }

        private void SettingsWindow_FormClosed(object sender, FormClosedEventArgs e) => FontManager.Cleanup();

        #region Settings Event Handlers

        private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (languageComboBox.SelectedIndex >= 0 && languageComboBox.SelectedIndex < LocalizationManager.AvailableLanguages.Length)
            {
                string selectedLangCode = LocalizationManager.AvailableLanguages[languageComboBox.SelectedIndex];
                SettingsManager.LauncherLanguage = languageComboBox.SelectedIndex;

                LocalizationManager.LoadLanguage(selectedLangCode);
                ApplyLocalization();
            }
        }

        private void FullscreenComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.FullScreenMode = fullscreenComboBox.SelectedIndex;
        }

        private void ResolutionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.ScreenResolution = resolutionComboBox.SelectedIndex;
        }

        private void GraphicsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SettingsManager.GraphicsQuality = graphicsComboBox.SelectedIndex;
        }

        private void SensitivitySlider_Scroll(object sender, EventArgs e)
        {
            int currentValue = sensitivitySlider.Value;
            sensitivityValueLabel.Text = currentValue.ToString();
            SettingsManager.MouseSensitivity = currentValue;
        }

        private void CustomFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = folderBrowserDialog.SelectedPath;
                customFolderTextBox.Text = selectedPath;
                SettingsManager.GamePath = selectedPath;
                OnCustomFolderChanged?.Invoke();
            }
        }

        #endregion

        #region Form Setup Methods

        private void SetupFormDrag()
        {
            MouseDown += (sender, e) =>
            {
                Capture = false;
                Message msg = Message.Create(Handle, 0xA1, new IntPtr(2), IntPtr.Zero);
                WndProc(ref msg);
            };
        }


        private void InitializeSettings()
        {
            LoadComboBoxSetting(languageComboBox, SettingsManager.LauncherLanguage);
            LoadComboBoxSetting(fullscreenComboBox, SettingsManager.FullScreenMode);
            LoadComboBoxSetting(graphicsComboBox, SettingsManager.GraphicsQuality);

            LoadResolutionSetting();
            LoadGamePathSetting();
            LoadMouseSensitivitySetting();
        }

        private void LoadComboBoxSetting(ComboBox comboBox, int settingValue)
        {
            if (settingValue == -1 ||
                settingValue < 0 ||
                settingValue >= comboBox.Items.Count)
            {
                comboBox.SelectedIndex = 0;
            }
            else
            {
                comboBox.SelectedIndex = settingValue;
            }
        }

        private void LoadResolutionSetting()
        {
            if (SettingsManager.ScreenResolution == -1)
            {
                SetDefaultResolution();
            }
            else
            {
                int index = SettingsManager.ScreenResolution;
                if (index >= 0 && index < resolutionComboBox.Items.Count)
                {
                    resolutionComboBox.SelectedIndex = index;
                }
                else
                {
                    SetDefaultResolution();
                }
            }
        }

        private void LoadGamePathSetting()
        {
            customFolderTextBox.Text = SettingsManager.GamePath ?? string.Empty;
        }

        private void LoadMouseSensitivitySetting()
        {
            sensitivitySlider.Value = SettingsManager.MouseSensitivity;
            sensitivityValueLabel.Text = SettingsManager.MouseSensitivity.ToString();
        }

        #endregion

        #region Resolution Methods

        private void SetDefaultResolution()
        {
            Screen primaryScreen = Screen.PrimaryScreen;
            string screenResolution = $"{primaryScreen.Bounds.Width}x{primaryScreen.Bounds.Height}";

            if (TryFindExactResolution(screenResolution)) return;

            SetClosestResolution(primaryScreen.Bounds.Width, primaryScreen.Bounds.Height);
        }

        private bool TryFindExactResolution(string targetResolution)
        {
            for (int i = 0; i < resolutionComboBox.Items.Count; i++)
            {
                if (resolutionComboBox.Items[i].ToString() == targetResolution)
                {
                    resolutionComboBox.SelectedIndex = i;
                    return true;
                }
            }
            return false;
        }

        private void SetClosestResolution(int width, int height)
        {
            var standardResolutions = GetStandardResolutions();
            string closestResolution = FindClosestResolution(standardResolutions, width, height);

            SelectResolutionFromComboBox(closestResolution);
        }

        private Dictionary<string, int> GetStandardResolutions()
        {
            return new Dictionary<string, int>
            {
                { "3840x2160", 3840 * 2160 },
                { "2560x1440", 2560 * 1440 },
                { "1920x1080", 1920 * 1080 },
                { "1600x900", 1600 * 900 },
                { "1366x768", 1366 * 768 },
                { "1280x720", 1280 * 720 }
            };
        }

        private string FindClosestResolution(Dictionary<string, int> resolutions, int width, int height)
        {
            int currentPixels = width * height;
            string closestResolution = "";
            int minDifference = int.MaxValue;

            foreach (var resolution in resolutions)
            {
                int difference = Math.Abs(currentPixels - resolution.Value);
                if (difference < minDifference)
                {
                    minDifference = difference;
                    closestResolution = resolution.Key;
                }
            }

            return closestResolution;
        }

        private void SelectResolutionFromComboBox(string resolution)
        {
            for (int i = 0; i < resolutionComboBox.Items.Count; i++)
            {
                if (resolutionComboBox.Items[i].ToString() == resolution)
                {
                    resolutionComboBox.SelectedIndex = i;
                    return;
                }
            }

            if (resolutionComboBox.Items.Count > 0) resolutionComboBox.SelectedIndex = 0;
        }

        #endregion
    }
}