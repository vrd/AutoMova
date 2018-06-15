namespace AutoSwitcher.UI
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
            this.components = new System.ComponentModel.Container();
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxConverLastHotkey = new System.Windows.Forms.TextBox();
            this.textBoxConvertSelectionHotkey = new System.Windows.Forms.TextBox();
            this.checkBoxSmartSelection = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoSwitching = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label7 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancelSettings
            // 
            this.buttonCancelSettings.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancelSettings.Location = new System.Drawing.Point(380, 302);
            this.buttonCancelSettings.Name = "buttonCancelSettings";
            this.buttonCancelSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonCancelSettings.TabIndex = 17;
            this.buttonCancelSettings.Text = "Cancel";
            this.buttonCancelSettings.UseVisualStyleBackColor = true;
            this.buttonCancelSettings.Click += new System.EventHandler(this.buttonCancelSettings_Click);
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(279, 302);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveSettings.TabIndex = 15;
            this.buttonSaveSettings.Text = "Apply";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonExit
            // 
            this.buttonExit.Location = new System.Drawing.Point(132, 302);
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
            this.checkBoxTrayIcon.Location = new System.Drawing.Point(12, 45);
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
            this.checkBoxAutorun.Location = new System.Drawing.Point(12, 22);
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
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Convert selection:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Convert last word:";
            // 
            // textBoxDelay
            // 
            this.textBoxDelay.Location = new System.Drawing.Point(336, 129);
            this.textBoxDelay.Name = "textBoxDelay";
            this.textBoxDelay.Size = new System.Drawing.Size(47, 20);
            this.textBoxDelay.TabIndex = 25;
            this.textBoxDelay.TextChanged += new System.EventHandler(this.textBoxDelay_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Delay before switching:";
            // 
            // buttonGithub
            // 
            this.buttonGithub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonGithub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGithub.Image = global::AutoSwitcher.Properties.Resources.github;
            this.buttonGithub.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonGithub.Location = new System.Drawing.Point(12, 302);
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
            this.textBoxSwitchLayoutHotkey.Location = new System.Drawing.Point(158, 67);
            this.textBoxSwitchLayoutHotkey.Name = "textBoxSwitchLayoutHotkey";
            this.textBoxSwitchLayoutHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxSwitchLayoutHotkey.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 29;
            this.label4.Text = "Switch keyboard layout:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxSwitchLayoutHotkey);
            this.groupBox1.Controls.Add(this.textBoxConverLastHotkey);
            this.groupBox1.Controls.Add(this.textBoxConvertSelectionHotkey);
            this.groupBox1.Location = new System.Drawing.Point(178, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 101);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hotkeys";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(132, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(13, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "?";
            this.label5.MouseLeave += new System.EventHandler(this.label5_MouseLeave);
            this.label5.MouseHover += new System.EventHandler(this.label5_MouseHover);
            // 
            // textBoxConverLastHotkey
            // 
            this.textBoxConverLastHotkey.Location = new System.Drawing.Point(158, 15);
            this.textBoxConverLastHotkey.Name = "textBoxConverLastHotkey";
            this.textBoxConverLastHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxConverLastHotkey.TabIndex = 21;
            // 
            // textBoxConvertSelectionHotkey
            // 
            this.textBoxConvertSelectionHotkey.Location = new System.Drawing.Point(158, 41);
            this.textBoxConvertSelectionHotkey.Name = "textBoxConvertSelectionHotkey";
            this.textBoxConvertSelectionHotkey.Size = new System.Drawing.Size(103, 20);
            this.textBoxConvertSelectionHotkey.TabIndex = 24;
            // 
            // checkBoxSmartSelection
            // 
            this.checkBoxSmartSelection.AutoSize = true;
            this.checkBoxSmartSelection.Checked = true;
            this.checkBoxSmartSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSmartSelection.Location = new System.Drawing.Point(12, 67);
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
            this.checkBoxAutoSwitching.Location = new System.Drawing.Point(11, 88);
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
            this.label6.Location = new System.Drawing.Point(12, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "This program is forked from";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Location = new System.Drawing.Point(142, 228);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(71, 13);
            this.linkLabel1.TabIndex = 0;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "dotSwitcher";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 100;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 250);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(318, 13);
            this.label7.TabIndex = 35;
            this.label7.Text = "Dictionaries are generated by Andrew Kuznetsov, author of great  ";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(321, 250);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(33, 13);
            this.linkLabel2.TabIndex = 36;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "xneur";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(350, 250);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "app";
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonSaveSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancelSettings;
            this.ClientSize = new System.Drawing.Size(468, 337);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.checkBoxAutoSwitching);
            this.Controls.Add(this.checkBoxSmartSelection);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonGithub);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxDelay);
            this.Controls.Add(this.checkBoxTrayIcon);
            this.Controls.Add(this.checkBoxAutorun);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonCancelSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "AutoSwitcher Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.Shown += new System.EventHandler(this.SettingsForm_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxConverLastHotkey;
        private System.Windows.Forms.TextBox textBoxConvertSelectionHotkey;
        private System.Windows.Forms.CheckBox checkBoxSmartSelection;
        private System.Windows.Forms.CheckBox checkBoxAutoSwitching;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label8;
    }
}