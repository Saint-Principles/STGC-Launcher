namespace STGCLauncher
{
    partial class SettingsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsWindow));
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageComboBox = new System.Windows.Forms.ComboBox();
            this.fullscreenLabel = new System.Windows.Forms.Label();
            this.fullscreenComboBox = new System.Windows.Forms.ComboBox();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.resolutionComboBox = new System.Windows.Forms.ComboBox();
            this.graphicsLabel = new System.Windows.Forms.Label();
            this.graphicsComboBox = new System.Windows.Forms.ComboBox();
            this.sensitivityLabel = new System.Windows.Forms.Label();
            this.sensitivityValueLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.customFolderButton = new System.Windows.Forms.Button();
            this.customFolderTextBox = new System.Windows.Forms.TextBox();
            this.customFolderLabel = new System.Windows.Forms.Label();
            this.exitButton = new System.Windows.Forms.Button();
            this.blackBox = new System.Windows.Forms.FlowLayoutPanel();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.sensitivitySlider = new Slider();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // languageLabel
            // 
            this.languageLabel.AutoSize = true;
            this.languageLabel.Font = new System.Drawing.Font("LD Slender", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.languageLabel.ForeColor = System.Drawing.Color.White;
            this.languageLabel.Location = new System.Drawing.Point(3, 15);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(121, 23);
            this.languageLabel.TabIndex = 1;
            this.languageLabel.Text = "Launcher Language:";
            // 
            // languageComboBox
            // 
            this.languageComboBox.BackColor = System.Drawing.Color.White;
            this.languageComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.languageComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.languageComboBox.ForeColor = System.Drawing.Color.Black;
            this.languageComboBox.FormattingEnabled = true;
            this.languageComboBox.Items.AddRange(new object[] {
            "English",
            "Русский"});
            this.languageComboBox.Location = new System.Drawing.Point(200, 15);
            this.languageComboBox.MaxDropDownItems = 50;
            this.languageComboBox.Name = "languageComboBox";
            this.languageComboBox.Size = new System.Drawing.Size(250, 23);
            this.languageComboBox.TabIndex = 1;
            this.languageComboBox.SelectedIndexChanged += new System.EventHandler(this.LanguageComboBox_SelectedIndexChanged);
            // 
            // fullscreenLabel
            // 
            this.fullscreenLabel.AutoSize = true;
            this.fullscreenLabel.Font = new System.Drawing.Font("LD Slender", 15.75F);
            this.fullscreenLabel.ForeColor = System.Drawing.Color.White;
            this.fullscreenLabel.Location = new System.Drawing.Point(3, 60);
            this.fullscreenLabel.Name = "fullscreenLabel";
            this.fullscreenLabel.Size = new System.Drawing.Size(104, 23);
            this.fullscreenLabel.TabIndex = 3;
            this.fullscreenLabel.Text = "Fullscreen Mode:";
            // 
            // fullscreenComboBox
            // 
            this.fullscreenComboBox.BackColor = System.Drawing.Color.White;
            this.fullscreenComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fullscreenComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.fullscreenComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.fullscreenComboBox.ForeColor = System.Drawing.Color.Black;
            this.fullscreenComboBox.FormattingEnabled = true;
            this.fullscreenComboBox.Items.AddRange(new object[] {
            "Fullscreen",
            "Windowed",
            "Fullscreen Windowed"});
            this.fullscreenComboBox.Location = new System.Drawing.Point(200, 60);
            this.fullscreenComboBox.Name = "fullscreenComboBox";
            this.fullscreenComboBox.Size = new System.Drawing.Size(250, 23);
            this.fullscreenComboBox.TabIndex = 2;
            this.fullscreenComboBox.SelectedIndexChanged += new System.EventHandler(this.FullscreenComboBox_SelectedIndexChanged);
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Font = new System.Drawing.Font("LD Slender", 15.75F);
            this.resolutionLabel.ForeColor = System.Drawing.Color.White;
            this.resolutionLabel.Location = new System.Drawing.Point(3, 105);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(118, 23);
            this.resolutionLabel.TabIndex = 5;
            this.resolutionLabel.Text = "Screen Resolution:";
            // 
            // resolutionComboBox
            // 
            this.resolutionComboBox.BackColor = System.Drawing.Color.White;
            this.resolutionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resolutionComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.resolutionComboBox.ForeColor = System.Drawing.Color.Black;
            this.resolutionComboBox.FormattingEnabled = true;
            this.resolutionComboBox.Items.AddRange(new object[] {
            "3840x2160",
            "3440x1440",
            "2560x1600",
            "2560x1440",
            "1920x1200",
            "1920x1080",
            "1680x1050",
            "1600x1200",
            "1600x900",
            "1440x900",
            "1366x768",
            "1280x1024",
            "1280x800",
            "1280x720",
            "1152x864",
            "1024x768",
            "800x600",
            "640x480"});
            this.resolutionComboBox.Location = new System.Drawing.Point(200, 105);
            this.resolutionComboBox.Name = "resolutionComboBox";
            this.resolutionComboBox.Size = new System.Drawing.Size(250, 23);
            this.resolutionComboBox.TabIndex = 3;
            this.resolutionComboBox.SelectedIndexChanged += new System.EventHandler(this.ResolutionComboBox_SelectedIndexChanged);
            // 
            // graphicsLabel
            // 
            this.graphicsLabel.AutoSize = true;
            this.graphicsLabel.Font = new System.Drawing.Font("LD Slender", 15.75F);
            this.graphicsLabel.ForeColor = System.Drawing.Color.White;
            this.graphicsLabel.Location = new System.Drawing.Point(3, 150);
            this.graphicsLabel.Name = "graphicsLabel";
            this.graphicsLabel.Size = new System.Drawing.Size(110, 23);
            this.graphicsLabel.TabIndex = 7;
            this.graphicsLabel.Text = "Graphics Quality:";
            // 
            // graphicsComboBox
            // 
            this.graphicsComboBox.BackColor = System.Drawing.Color.White;
            this.graphicsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.graphicsComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.graphicsComboBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.graphicsComboBox.ForeColor = System.Drawing.Color.Black;
            this.graphicsComboBox.FormattingEnabled = true;
            this.graphicsComboBox.Items.AddRange(new object[] {
            "Lowest",
            "Very Low",
            "Low",
            "Medium",
            "High",
            "Very High",
            "Highest"});
            this.graphicsComboBox.Location = new System.Drawing.Point(200, 150);
            this.graphicsComboBox.Name = "graphicsComboBox";
            this.graphicsComboBox.Size = new System.Drawing.Size(250, 23);
            this.graphicsComboBox.TabIndex = 4;
            this.graphicsComboBox.SelectedIndexChanged += new System.EventHandler(this.GraphicsComboBox_SelectedIndexChanged);
            // 
            // sensitivityLabel
            // 
            this.sensitivityLabel.AutoSize = true;
            this.sensitivityLabel.Font = new System.Drawing.Font("LD Slender", 15.75F);
            this.sensitivityLabel.ForeColor = System.Drawing.Color.White;
            this.sensitivityLabel.Location = new System.Drawing.Point(3, 195);
            this.sensitivityLabel.Name = "sensitivityLabel";
            this.sensitivityLabel.Size = new System.Drawing.Size(117, 23);
            this.sensitivityLabel.TabIndex = 9;
            this.sensitivityLabel.Text = "Mouse Sensitivity:";
            // 
            // sensitivityValueLabel
            // 
            this.sensitivityValueLabel.Font = new System.Drawing.Font("LD Slender", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sensitivityValueLabel.ForeColor = System.Drawing.Color.White;
            this.sensitivityValueLabel.Location = new System.Drawing.Point(416, 195);
            this.sensitivityValueLabel.Name = "sensitivityValueLabel";
            this.sensitivityValueLabel.Size = new System.Drawing.Size(34, 19);
            this.sensitivityValueLabel.TabIndex = 11;
            this.sensitivityValueLabel.Text = "0";
            this.sensitivityValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(120)))), ((int)(((byte)(70)))));
            this.saveButton.FlatAppearance.BorderSize = 0;
            this.saveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.saveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveButton.Font = new System.Drawing.Font("LD Slender", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Location = new System.Drawing.Point(30, 327);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(150, 40);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "SAVE";
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // resetButton
            // 
            this.resetButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.resetButton.FlatAppearance.BorderSize = 0;
            this.resetButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.resetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetButton.Font = new System.Drawing.Font("LD Slender", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetButton.ForeColor = System.Drawing.Color.White;
            this.resetButton.Location = new System.Drawing.Point(290, 327);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(150, 40);
            this.resetButton.TabIndex = 8;
            this.resetButton.Text = "RESET";
            this.resetButton.UseVisualStyleBackColor = false;
            this.resetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.BackColor = System.Drawing.Color.Transparent;
            this.settingsPanel.Controls.Add(this.customFolderButton);
            this.settingsPanel.Controls.Add(this.customFolderTextBox);
            this.settingsPanel.Controls.Add(this.customFolderLabel);
            this.settingsPanel.Controls.Add(this.languageLabel);
            this.settingsPanel.Controls.Add(this.languageComboBox);
            this.settingsPanel.Controls.Add(this.fullscreenLabel);
            this.settingsPanel.Controls.Add(this.fullscreenComboBox);
            this.settingsPanel.Controls.Add(this.sensitivityValueLabel);
            this.settingsPanel.Controls.Add(this.resolutionLabel);
            this.settingsPanel.Controls.Add(this.sensitivitySlider);
            this.settingsPanel.Controls.Add(this.resolutionComboBox);
            this.settingsPanel.Controls.Add(this.sensitivityLabel);
            this.settingsPanel.Controls.Add(this.graphicsLabel);
            this.settingsPanel.Controls.Add(this.graphicsComboBox);
            this.settingsPanel.Location = new System.Drawing.Point(12, 28);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(456, 280);
            this.settingsPanel.TabIndex = 1;
            // 
            // customFolderButton
            // 
            this.customFolderButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(90)))));
            this.customFolderButton.FlatAppearance.BorderSize = 0;
            this.customFolderButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.customFolderButton.Font = new System.Drawing.Font("LD Slender", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customFolderButton.ForeColor = System.Drawing.Color.White;
            this.customFolderButton.Location = new System.Drawing.Point(420, 240);
            this.customFolderButton.Name = "customFolderButton";
            this.customFolderButton.Size = new System.Drawing.Size(30, 23);
            this.customFolderButton.TabIndex = 6;
            this.customFolderButton.Text = "...";
            this.customFolderButton.UseVisualStyleBackColor = false;
            this.customFolderButton.Click += new System.EventHandler(this.CustomFolderButton_Click);
            // 
            // customFolderTextBox
            // 
            this.customFolderTextBox.BackColor = System.Drawing.Color.White;
            this.customFolderTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.customFolderTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.customFolderTextBox.ForeColor = System.Drawing.Color.Black;
            this.customFolderTextBox.Location = new System.Drawing.Point(200, 240);
            this.customFolderTextBox.Name = "customFolderTextBox";
            this.customFolderTextBox.ReadOnly = true;
            this.customFolderTextBox.Size = new System.Drawing.Size(214, 23);
            this.customFolderTextBox.TabIndex = 6;
            // 
            // customFolderLabel
            // 
            this.customFolderLabel.AutoSize = true;
            this.customFolderLabel.Font = new System.Drawing.Font("LD Slender", 15.75F);
            this.customFolderLabel.ForeColor = System.Drawing.Color.White;
            this.customFolderLabel.Location = new System.Drawing.Point(3, 240);
            this.customFolderLabel.Name = "customFolderLabel";
            this.customFolderLabel.Size = new System.Drawing.Size(95, 23);
            this.customFolderLabel.TabIndex = 12;
            this.customFolderLabel.Text = "Custom Folder:";
            // 
            // exitButton
            // 
            this.exitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exitButton.BackColor = System.Drawing.Color.Transparent;
            this.exitButton.FlatAppearance.BorderSize = 0;
            this.exitButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exitButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.exitButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exitButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.exitButton.ForeColor = System.Drawing.Color.White;
            this.exitButton.Location = new System.Drawing.Point(451, 0);
            this.exitButton.MaximumSize = new System.Drawing.Size(28, 28);
            this.exitButton.MinimumSize = new System.Drawing.Size(28, 28);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(28, 28);
            this.exitButton.TabIndex = 9;
            this.exitButton.Text = "X";
            this.exitButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // blackBox
            // 
            this.blackBox.BackColor = System.Drawing.Color.Black;
            this.blackBox.Location = new System.Drawing.Point(-2, 315);
            this.blackBox.Name = "blackBox";
            this.blackBox.Size = new System.Drawing.Size(482, 66);
            this.blackBox.TabIndex = 13;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Select custom folder for STGC";
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // sensitivitySlider
            // 
            this.sensitivitySlider.BackColor = System.Drawing.Color.Transparent;
            this.sensitivitySlider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.sensitivitySlider.Location = new System.Drawing.Point(200, 195);
            this.sensitivitySlider.Maximum = 5;
            this.sensitivitySlider.Minimum = -5;
            this.sensitivitySlider.Name = "sensitivitySlider";
            this.sensitivitySlider.Size = new System.Drawing.Size(210, 25);
            this.sensitivitySlider.SliderColor = System.Drawing.Color.White;
            this.sensitivitySlider.TabIndex = 5;
            this.sensitivitySlider.TrackColor = System.Drawing.Color.DimGray;
            this.sensitivitySlider.Value = 0;
            this.sensitivitySlider.ValueChanged += new System.EventHandler(this.SensitivitySlider_Scroll);
            // 
            // SettingsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.BackgroundImage = global::STGCLauncher.Properties.Resources.Launcher_Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(480, 380);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.blackBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsWindow_FormClosed);
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label languageLabel;
        private System.Windows.Forms.ComboBox languageComboBox;
        private System.Windows.Forms.Label fullscreenLabel;
        private System.Windows.Forms.ComboBox fullscreenComboBox;
        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.ComboBox resolutionComboBox;
        private System.Windows.Forms.Label graphicsLabel;
        private System.Windows.Forms.ComboBox graphicsComboBox;
        private System.Windows.Forms.Label sensitivityLabel;
        private Slider sensitivitySlider;
        private System.Windows.Forms.Label sensitivityValueLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Label customFolderLabel;
        private System.Windows.Forms.TextBox customFolderTextBox;
        private System.Windows.Forms.Button customFolderButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.FlowLayoutPanel blackBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}