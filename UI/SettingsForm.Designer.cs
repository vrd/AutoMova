namespace AutoMova.UI
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonCancelSettings = new System.Windows.Forms.Button();
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonExit = new System.Windows.Forms.Button();
            this.checkBoxTrayIcon = new System.Windows.Forms.CheckBox();
            this.checkBoxAutorun = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDelay = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonGithub = new System.Windows.Forms.Button();
            this.textBoxSwitchLayoutHotkey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxConverLastHotkey = new System.Windows.Forms.TextBox();
            this.textBoxConvertSelectionHotkey = new System.Windows.Forms.TextBox();
            this.checkBoxSmartSelection = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSwitching = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label7 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAutoSwitchingHotkey = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.label13 = new System.Windows.Forms.Label();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.textBoxDebug = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancelSettings
            // 
            this.buttonCancelSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelSettings.Location = new System.Drawing.Point(322, 337);
            this.buttonCancelSettings.Name = "buttonCancelSettings";
            this.buttonCancelSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelSettings.TabIndex = 17;
            this.buttonCancelSettings.Text = "Cancel";
            this.buttonCancelSettings.UseVisualStyleBackColor = true;
            this.buttonCancelSettings.Click += new System.EventHandler(this.buttonCancelSettings_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(241, 337);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 15;
            this.buttonSaveSettings.Text = "OK";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(12, 337);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 18;
            this.buttonExit.Text = "Exit program";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // checkBoxTrayIcon
            // 
            this.checkBoxTrayIcon.AutoSize = true;
            this.checkBoxTrayIcon.Location = new System.Drawing.Point(6, 29);
            this.checkBoxTrayIcon.Name = "checkBoxTrayIcon";
            this.checkBoxTrayIcon.Size = new System.Drawing.Size(96, 17);
            this.checkBoxTrayIcon.TabIndex = 20;
            this.checkBoxTrayIcon.Text = "Show tray icon";
            this.checkBoxTrayIcon.UseVisualStyleBackColor = true;
            this.checkBoxTrayIcon.CheckedChanged += new System.EventHandler(this.checkBoxTrayIcon_CheckedChanged);
            // 
            // checkBoxAutorun
            // 
            this.checkBoxAutorun.AutoSize = true;
            this.checkBoxAutorun.Location = new System.Drawing.Point(6, 6);
            this.checkBoxAutorun.Name = "checkBoxAutorun";
            this.checkBoxAutorun.Size = new System.Drawing.Size(145, 17);
            this.checkBoxAutorun.TabIndex = 19;
            this.checkBoxAutorun.Text = "Start on Windows startup";
            this.checkBoxAutorun.UseVisualStyleBackColor = true;
            this.checkBoxAutorun.CheckedChanged += new System.EventHandler(this.checkBoxAutorun_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Convert selection:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Convert last word:";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(158, 11);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(47, 20);
            this.textBoxDelay.TabIndex = 25;
            this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxDelay_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Delay before switching:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // buttonGithub
            // 
            this.buttonGithub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonGithub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGithub.Image = global::AutoMova.Properties.Resources.github;
            this.buttonGithub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonGithub.Location = new System.Drawing.Point(126, 78);
            this.buttonGithub.Name = "buttonGithub";
            this.buttonGithub.Size = new System.Drawing.Size(114, 23);
            this.buttonGithub.TabIndex = 28;
            this.buttonGithub.Text = "Report an issue";
            this.buttonGithub.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonGithub.UseVisualStyleBackColor = true;
            this.buttonGithub.Click += new System.EventHandler(this.buttonGithub_Click);
            // 
            // textBoxSwitchLayoutHotkey
            // 
            this.textBoxSwitchLayoutHotkey.Location = new System.Drawing.Point(158, 62);
            this.textBoxSwitchLayoutHotkey.Name = "textBoxSwitchLayoutHotkey";
            this.textBoxSwitchLayoutHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxSwitchLayoutHotkey.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Switch keyboard layout:";
            // 
            // textBoxConverLastHotkey
            // 
            this.textBoxConverLastHotkey.Location = new System.Drawing.Point(158, 10);
            this.textBoxConverLastHotkey.Name = "textBoxConverLastHotkey";
            this.textBoxConverLastHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxConverLastHotkey.TabIndex = 21;
            // 
            // textBoxConvertSelectionHotkey
            // 
            this.textBoxConvertSelectionHotkey.Location = new System.Drawing.Point(158, 36);
            this.textBoxConvertSelectionHotkey.Name = "textBoxConvertSelectionHotkey";
            this.textBoxConvertSelectionHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxConvertSelectionHotkey.TabIndex = 24;
            // 
            // checkBoxSmartSelection
            // 
            this.checkBoxSmartSelection.AutoCheck = false;
            this.checkBoxSmartSelection.AutoSize = true;
            this.checkBoxSmartSelection.Enabled = false;
            this.checkBoxSmartSelection.Location = new System.Drawing.Point(5, 51);
            this.checkBoxSmartSelection.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxSmartSelection.Name = "checkBoxSmartSelection";
            this.checkBoxSmartSelection.Size = new System.Drawing.Size(157, 17);
            this.checkBoxSmartSelection.TabIndex = 33;
            this.checkBoxSmartSelection.Text = "Use smart convert selection";
            this.checkBoxSmartSelection.UseVisualStyleBackColor = true;
            this.checkBoxSmartSelection.CheckedChanged += new System.EventHandler(this.smartSelection_CheckedChanged);
            // 
            // checkBoxAutoSwitching
            // 
            this.checkBoxAutoSwitching.AutoSize = true;
            this.checkBoxAutoSwitching.Checked = true;
            this.checkBoxAutoSwitching.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoSwitching.Location = new System.Drawing.Point(5, 72);
            this.checkBoxAutoSwitching.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxAutoSwitching.Name = "checkBoxAutoSwitching";
            this.checkBoxAutoSwitching.Size = new System.Drawing.Size(120, 17);
            this.checkBoxAutoSwitching.TabIndex = 34;
            this.checkBoxAutoSwitching.Text = "Automatic switching";
            this.checkBoxAutoSwitching.UseVisualStyleBackColor = true;
            this.checkBoxAutoSwitching.CheckedChanged += new System.EventHandler(this.autoSwitching_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "This program is forked from";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(137, 15);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(71, 13);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "dotSwitcher";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 37);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(344, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Proto dictionaries are generated by Andrew Kuznetsov, author of great  ";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(315, 37);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(33, 13);
            this.linkLabel2.TabIndex = 36;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "xneur";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(345, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(0, 13);
            this.label8.TabIndex = 37;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(386, 319);
            this.tabControl1.TabIndex = 38;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxSmartSelection);
            this.tabPage1.Controls.Add(this.checkBoxAutorun);
            this.tabPage1.Controls.Add(this.checkBoxTrayIcon);
            this.tabPage1.Controls.Add(this.checkBoxAutoSwitching);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(378, 293);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.textBoxAutoSwitchingHotkey);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.textBoxConvertSelectionHotkey);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBoxConverLastHotkey);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.textBoxSwitchLayoutHotkey);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(378, 293);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Hotkeys";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "Toggle automatic mode:";
            // 
            // textBoxAutoSwitchingHotkey
            // 
            this.textBoxAutoSwitchingHotkey.Location = new System.Drawing.Point(158, 88);
            this.textBoxAutoSwitchingHotkey.Name = "textBoxAutoSwitchingHotkey";
            this.textBoxAutoSwitchingHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxAutoSwitchingHotkey.TabIndex = 32;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.textBoxDelay);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(378, 293);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Misc";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.linkLabel5);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Controls.Add(this.linkLabel4);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Controls.Add(this.linkLabel3);
            this.tabPage4.Controls.Add(this.label11);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.linkLabel1);
            this.tabPage4.Controls.Add(this.linkLabel2);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(378, 293);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Credits";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // linkLabel5
            // 
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Location = new System.Drawing.Point(159, 84);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(65, 13);
            this.linkLabel5.TabIndex = 43;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "Andriy Rysin";
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 84);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(156, 13);
            this.label13.TabIndex = 42;
            this.label13.Text = "Ukrainian hunspell dictionary by";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Location = new System.Drawing.Point(156, 60);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(46, 13);
            this.linkLabel4.TabIndex = 41;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "SCOWL";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(153, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "English hunspell dictionaries by";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(152, 108);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(97, 13);
            this.linkLabel3.TabIndex = 39;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Aleksandr Klyukvin";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(149, 13);
            this.label11.TabIndex = 38;
            this.label11.Text = "Russian hunspell dictionary by";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label10);
            this.tabPage5.Controls.Add(this.label9);
            this.tabPage5.Controls.Add(this.buttonGithub);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(378, 293);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "About";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(96, 51);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(174, 13);
            this.label10.TabIndex = 30;
            this.label10.Text = "Automatic keyboard layout switcher";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label9.Location = new System.Drawing.Point(151, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 29;
            this.label9.Text = "AutoMova";
            this.label9.Click += new System.EventHandler(this.label9_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.textBoxDebug);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(378, 293);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Debug";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // textBoxDebug
            // 
            this.textBoxDebug.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxDebug.Location = new System.Drawing.Point(6, 6);
            this.textBoxDebug.Multiline = true;
            this.textBoxDebug.Name = "textBoxDebug";
            this.textBoxDebug.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxDebug.Size = new System.Drawing.Size(366, 281);
            this.textBoxDebug.TabIndex = 0;
            this.textBoxDebug.WordWrap = false;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSaveSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelSettings;
            this.ClientSize = new System.Drawing.Size(410, 373);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonCancelSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.Text = "AutoMova Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCancelSettings;
        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.CheckBox checkBoxTrayIcon;
        private System.Windows.Forms.CheckBox checkBoxAutorun;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonGithub;
        private System.Windows.Forms.TextBox textBoxSwitchLayoutHotkey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxConverLastHotkey;
        private System.Windows.Forms.TextBox textBoxConvertSelectionHotkey;
        private System.Windows.Forms.CheckBox checkBoxSmartSelection;
        private System.Windows.Forms.CheckBox checkBoxAutoSwitching;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox textBoxDebug;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAutoSwitchingHotkey;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.Label label13;
    }
}