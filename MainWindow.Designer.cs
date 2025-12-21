namespace STGCLauncher
{
    partial class MainWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.blackBox = new System.Windows.Forms.Panel();
            this.updateProgressLabel = new System.Windows.Forms.Label();
            this.progressBarBackground = new System.Windows.Forms.Panel();
            this.progressBar = new System.Windows.Forms.Label();
            this.settingsButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.logoPicture = new System.Windows.Forms.PictureBox();
            this.antenna = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.newsImage = new System.Windows.Forms.PictureBox();
            this.newsTextLabel = new System.Windows.Forms.Label();
            this.newsTitleLabel = new System.Windows.Forms.Label();
            this.updatePanel = new System.Windows.Forms.Panel();
            this.updateLabel2 = new System.Windows.Forms.Label();
            this.updateLabel1 = new System.Windows.Forms.Label();
            this.blackBox.SuspendLayout();
            this.progressBarBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.antenna)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newsImage)).BeginInit();
            this.updatePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // blackBox
            // 
            this.blackBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blackBox.BackColor = System.Drawing.Color.Black;
            this.blackBox.Controls.Add(this.updateProgressLabel);
            this.blackBox.Controls.Add(this.progressBarBackground);
            this.blackBox.Controls.Add(this.settingsButton);
            this.blackBox.Controls.Add(this.statusLabel);
            this.blackBox.Controls.Add(this.startButton);
            this.blackBox.Location = new System.Drawing.Point(0, 346);
            this.blackBox.Name = "blackBox";
            this.blackBox.Size = new System.Drawing.Size(784, 69);
            this.blackBox.TabIndex = 2;
            // 
            // updateProgressLabel
            // 
            this.updateProgressLabel.BackColor = System.Drawing.Color.Transparent;
            this.updateProgressLabel.Font = new System.Drawing.Font("LD Slender", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateProgressLabel.ForeColor = System.Drawing.Color.Gray;
            this.updateProgressLabel.Location = new System.Drawing.Point(12, 2);
            this.updateProgressLabel.Name = "updateProgressLabel";
            this.updateProgressLabel.Size = new System.Drawing.Size(147, 17);
            this.updateProgressLabel.TabIndex = 12;
            this.updateProgressLabel.Text = "Update Progress";
            // 
            // progressBarBackground
            // 
            this.progressBarBackground.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.progressBarBackground.Controls.Add(this.progressBar);
            this.progressBarBackground.ForeColor = System.Drawing.Color.White;
            this.progressBarBackground.Location = new System.Drawing.Point(12, 22);
            this.progressBarBackground.Name = "progressBarBackground";
            this.progressBarBackground.Size = new System.Drawing.Size(410, 24);
            this.progressBarBackground.TabIndex = 7;
            this.progressBarBackground.Visible = false;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(209)))), ((int)(((byte)(103)))), ((int)(((byte)(106)))));
            this.progressBar.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.progressBar.ForeColor = System.Drawing.Color.White;
            this.progressBar.Location = new System.Drawing.Point(3, 3);
            this.progressBar.Margin = new System.Windows.Forms.Padding(3);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(404, 18);
            this.progressBar.TabIndex = 0;
            this.progressBar.Text = "100%";
            this.progressBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // settingsButton
            // 
            this.settingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsButton.BackColor = System.Drawing.Color.DimGray;
            this.settingsButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.settingsButton.FlatAppearance.BorderSize = 0;
            this.settingsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.settingsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.settingsButton.Font = new System.Drawing.Font("LD Slender", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsButton.ForeColor = System.Drawing.Color.White;
            this.settingsButton.Image = global::STGCLauncher.Properties.Resources.Gear_Icon;
            this.settingsButton.Location = new System.Drawing.Point(728, 12);
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(44, 41);
            this.settingsButton.TabIndex = 6;
            this.settingsButton.UseVisualStyleBackColor = false;
            this.settingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Font = new System.Drawing.Font("LD Slender", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.Gray;
            this.statusLabel.Location = new System.Drawing.Point(14, 12);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(522, 41);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "A new update is available!";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // startButton
            // 
            this.startButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.startButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(179)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.startButton.Cursor = System.Windows.Forms.Cursors.Default;
            this.startButton.FlatAppearance.BorderSize = 0;
            this.startButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkGray;
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Font = new System.Drawing.Font("LD Slender", 18F);
            this.startButton.ForeColor = System.Drawing.Color.White;
            this.startButton.Location = new System.Drawing.Point(542, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(180, 41);
            this.startButton.TabIndex = 4;
            this.startButton.Text = "UPDATE NOW";
            this.startButton.UseVisualStyleBackColor = false;
            this.startButton.Click += new System.EventHandler(this.StartButton_Click);
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
            this.exitButton.Location = new System.Drawing.Point(756, 0);
            this.exitButton.MaximumSize = new System.Drawing.Size(28, 28);
            this.exitButton.MinimumSize = new System.Drawing.Size(28, 28);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(28, 28);
            this.exitButton.TabIndex = 5;
            this.exitButton.Text = "X";
            this.exitButton.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.exitButton.UseVisualStyleBackColor = false;
            this.exitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // logoPicture
            // 
            this.logoPicture.BackColor = System.Drawing.Color.Transparent;
            this.logoPicture.Image = global::STGCLauncher.Properties.Resources.Logo;
            this.logoPicture.Location = new System.Drawing.Point(8, 12);
            this.logoPicture.Name = "logoPicture";
            this.logoPicture.Size = new System.Drawing.Size(411, 105);
            this.logoPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logoPicture.TabIndex = 6;
            this.logoPicture.TabStop = false;
            // 
            // antenna
            // 
            this.antenna.BackColor = System.Drawing.Color.Transparent;
            this.antenna.Image = global::STGCLauncher.Properties.Resources.Antenna;
            this.antenna.InitialImage = global::STGCLauncher.Properties.Resources.Antenna;
            this.antenna.Location = new System.Drawing.Point(-8, 287);
            this.antenna.Name = "antenna";
            this.antenna.Size = new System.Drawing.Size(96, 65);
            this.antenna.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.antenna.TabIndex = 3;
            this.antenna.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Font = new System.Drawing.Font("LD Slender", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.versionLabel.Location = new System.Drawing.Point(684, 325);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(88, 18);
            this.versionLabel.TabIndex = 7;
            this.versionLabel.Text = "v0.1";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // newsImage
            // 
            this.newsImage.BackColor = System.Drawing.Color.Transparent;
            this.newsImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.newsImage.Location = new System.Drawing.Point(472, 120);
            this.newsImage.Name = "newsImage";
            this.newsImage.Size = new System.Drawing.Size(300, 161);
            this.newsImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.newsImage.TabIndex = 8;
            this.newsImage.TabStop = false;
            // 
            // newsTextLabel
            // 
            this.newsTextLabel.BackColor = System.Drawing.Color.Transparent;
            this.newsTextLabel.Font = new System.Drawing.Font("LD Slender", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newsTextLabel.ForeColor = System.Drawing.Color.White;
            this.newsTextLabel.Image = global::STGCLauncher.Properties.Resources.Box;
            this.newsTextLabel.Location = new System.Drawing.Point(18, 154);
            this.newsTextLabel.Name = "newsTextLabel";
            this.newsTextLabel.Size = new System.Drawing.Size(387, 127);
            this.newsTextLabel.TabIndex = 2;
            this.newsTextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newsTextLabel.Visible = false;
            // 
            // newsTitleLabel
            // 
            this.newsTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.newsTitleLabel.Font = new System.Drawing.Font("LD Slender", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newsTitleLabel.ForeColor = System.Drawing.Color.White;
            this.newsTitleLabel.Location = new System.Drawing.Point(8, 120);
            this.newsTitleLabel.Name = "newsTitleLabel";
            this.newsTitleLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.newsTitleLabel.Size = new System.Drawing.Size(411, 34);
            this.newsTitleLabel.TabIndex = 1;
            this.newsTitleLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // updatePanel
            // 
            this.updatePanel.BackColor = System.Drawing.Color.Transparent;
            this.updatePanel.BackgroundImage = global::STGCLauncher.Properties.Resources.Box;
            this.updatePanel.Controls.Add(this.updateLabel2);
            this.updatePanel.Controls.Add(this.updateLabel1);
            this.updatePanel.ForeColor = System.Drawing.Color.Transparent;
            this.updatePanel.Location = new System.Drawing.Point(0, 0);
            this.updatePanel.Name = "updatePanel";
            this.updatePanel.Size = new System.Drawing.Size(784, 412);
            this.updatePanel.TabIndex = 9;
            this.updatePanel.Visible = false;
            // 
            // updateLabel2
            // 
            this.updateLabel2.AutoSize = true;
            this.updateLabel2.Font = new System.Drawing.Font("LD Slender", 16F);
            this.updateLabel2.ForeColor = System.Drawing.Color.White;
            this.updateLabel2.Location = new System.Drawing.Point(173, 194);
            this.updateLabel2.Name = "updateLabel2";
            this.updateLabel2.Size = new System.Drawing.Size(439, 48);
            this.updateLabel2.TabIndex = 1;
            this.updateLabel2.Text = "An important update is downloading. Please do not close the launcher. \r\nIt will a" +
    "utomatically restart in a few seconds.";
            this.updateLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // updateLabel1
            // 
            this.updateLabel1.AutoSize = true;
            this.updateLabel1.Font = new System.Drawing.Font("LD Slender", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateLabel1.ForeColor = System.Drawing.Color.White;
            this.updateLabel1.Location = new System.Drawing.Point(292, 154);
            this.updateLabel1.Name = "updateLabel1";
            this.updateLabel1.Size = new System.Drawing.Size(197, 32);
            this.updateLabel1.TabIndex = 0;
            this.updateLabel1.Text = "Launcher is updating...";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.BackgroundImage = global::STGCLauncher.Properties.Resources.Launcher_Background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.newsTitleLabel);
            this.Controls.Add(this.newsTextLabel);
            this.Controls.Add(this.newsImage);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.blackBox);
            this.Controls.Add(this.antenna);
            this.Controls.Add(this.logoPicture);
            this.Controls.Add(this.updatePanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STGCLauncher";
            this.Load += new System.EventHandler(this.Window_Load);
            this.blackBox.ResumeLayout(false);
            this.progressBarBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.logoPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.antenna)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newsImage)).EndInit();
            this.updatePanel.ResumeLayout(false);
            this.updatePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel blackBox;
        private System.Windows.Forms.PictureBox antenna;
        private System.Windows.Forms.PictureBox newsImage;
        private System.Windows.Forms.PictureBox logoPicture;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button exitButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Panel progressBarBackground;
        private System.Windows.Forms.Label progressBar;
        private System.Windows.Forms.Label updateProgressLabel;
        private System.Windows.Forms.Label newsTextLabel;
        private System.Windows.Forms.Label newsTitleLabel;
        private System.Windows.Forms.Panel updatePanel;
        private System.Windows.Forms.Label updateLabel2;
        private System.Windows.Forms.Label updateLabel1;
    }
}

