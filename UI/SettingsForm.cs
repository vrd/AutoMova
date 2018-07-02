using AutoMova.Data;
using AutoMova.Switcher;
using AutoMova.WinApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMova.UI
{
    public partial class SettingsForm : Form
    {
        public event EventHandler<EventArgs> Exit;
        Settings settings;
        SwitcherCore engine;

        public SettingsForm(Settings settings, SwitcherCore engine)
        {
            this.settings = settings;
            this.engine = engine;
            engine.Error += OnEngineError;
            
            InitializeComponent();
            InitializeTrayIcon();
            InitializeHotkeyBoxes();

            UpdateUi();
        }

        

        /**
         * SETTINGS FORM
         */
        void ShowForm()
        {
            engine.Stop();
            kbdHook.Start();
            icon.Hide();
            TopMost = true;
            UpdateUi();
            Show();
        }
        void HideForm()
        {
            kbdHook.Stop();
            engine.Start();
            if (settings.ShowTrayIcon == true)
            {
                icon.Show();
            }
            TopMost = false;
            UpdateUi();
            Hide();
        }
        // Update input values and icon state
        void UpdateUi()
        {
            textBoxConverLastHotkey.Text = ReplaceCtrls(settings.ConvertLastHotkey.ToString());
            textBoxConvertSelectionHotkey.Text = settings.ConvertSelectionHotkey.ToString();
            textBoxSwitchLayoutHotkey.Text = ReplaceCtrls(settings.SwitchLayoutHotkey.ToString());
            checkBoxAutorun.Checked = settings.AutoStart == true;
            checkBoxTrayIcon.Checked = settings.ShowTrayIcon == true;
            checkBoxSmartSelection.Checked = settings.SmartSelection == true;
            checkBoxAutoSwitching.Checked = settings.AutoSwitching == true;
            DisplaySwitchDelay(settings.SwitchDelay);
            icon.SetRunning(engine.IsStarted());
        }
        // also ESC
        void buttonCancelSettings_Click(object sender, EventArgs e)
        {
            ResetSettings();
            HideForm();
        }
        void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            if (settings.ConvertLastHotkey.Alt || settings.ConvertLastHotkey.Win)
            {
                MessageBox.Show("Sorry, win+ and alt+ hotkeys are not supported yet");
                return;
            }
            SaveSettings();
            HideForm();
        }
        void buttonExit_Click(object sender, EventArgs e)
        {
            ResetSettings();
            OnExit();
        }
        // initial hidden state
        void SettingsForm_Shown(object sender, EventArgs e)
        {
            HideForm();
        }
        // prevent the form from user-initiated closing ([x] click, Alt+F4, closing from taskbar)
        // (acts as Cancel)
        void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
            {
                return;
            }

            e.Cancel = true;
            ResetSettings();
            HideForm();
        }     
        // receive window message from another instance
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == LowLevelAdapter.WM_SHOW_SETTINGS)
            {
                ShowForm();
            }
            base.WndProc(ref m);
        }

        /**
         * TRAY ICON
         */
        TrayIcon icon;
        void InitializeTrayIcon()
        {
            icon = new TrayIcon(settings.ShowTrayIcon);
            icon.DoubleClick += icon_SettingsClick;
            icon.ExitClick += icon_ExitClick;
            icon.SettingsClick += icon_SettingsClick;
            icon.TogglePowerClick += icon_TogglePowerClick;
        }
        void icon_TogglePowerClick(object sender, EventArgs e)
        {
            if (engine.IsStarted())
            {
                engine.Stop();
                UpdateUi();
            }
            else
            {
                engine.Start();
                UpdateUi();
            }
        }
        void icon_SettingsClick(object sender, EventArgs e)
        {
            ShowForm();
        }
        void icon_ExitClick(object sender, EventArgs e)
        {
            OnExit();
        }
        void OnEngineError(object sender, SwitcherErrorArgs e)
        {
            icon.ShowTooltip(e.Error.Message, ToolTipIcon.Error);
        }
        
        /**
         * HOTKEY INPUTS
         */
        KeyboardHook kbdHook;
        KeyboardEventArgs currentHotkey;
        HotKeyType currentHotkeyType;
        enum HotKeyType { None, ConvertLast, ConvertSelection, SwitchLayout }
        void InitializeHotkeyBoxes()
        {
            textBoxConverLastHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.ConvertLast;
            textBoxConverLastHotkey.Enter += (s, e) => currentHotkeyType = HotKeyType.ConvertLast;
            textBoxConverLastHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();
            textBoxConverLastHotkey.Leave += (s, e) => ApplyCurrentHotkey();
            textBoxConvertSelectionHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.ConvertSelection;
            textBoxConvertSelectionHotkey.Enter += (s, e) => currentHotkeyType = HotKeyType.ConvertSelection;
            textBoxConvertSelectionHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();
            textBoxConvertSelectionHotkey.Leave += (s, e) => ApplyCurrentHotkey();
            textBoxSwitchLayoutHotkey.GotFocus += (s, e) => currentHotkeyType = HotKeyType.SwitchLayout;
            textBoxSwitchLayoutHotkey.Enter += (s, e) => currentHotkeyType = HotKeyType.SwitchLayout;
            textBoxSwitchLayoutHotkey.LostFocus += (s, e) => ApplyCurrentHotkey();
            textBoxSwitchLayoutHotkey.Leave += (s, e) => ApplyCurrentHotkey();
            currentHotkeyType = HotKeyType.None;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += kbdHook_KeyboardEvent;
        }
        void kbdHook_KeyboardEvent(object sender, KeyboardEventArgs e)
        {
            if (currentHotkeyType != HotKeyType.None && e.Type == KeyboardEventType.KeyDown)
            {
                var vk = e.KeyCode;
                if (vk == Keys.Escape || vk == Keys.Back)
                {
                    e.Handled = true;
                    ResetCurrentHotkey(vk == Keys.Back);
                    return;
                }
                if (vk != Keys.LMenu && vk != Keys.RMenu
                    && vk != Keys.LWin && vk != Keys.RWin
                    && vk != Keys.LShiftKey && vk != Keys.RShiftKey
                    && vk != Keys.LControlKey && vk != Keys.RControlKey)
                {
                    e.Handled = true;
                }
                SetCurrentHotkeyInputText(e.ToString());
                currentHotkey = e;
            }
        }
        // TODO: refactor this (make HotkeyInput : TextBox)
        void SetCurrentHotkeyInputText(string text)
        {
            TextBox currentTextBox;
            switch (currentHotkeyType)
            {
                case HotKeyType.ConvertLast:
                    currentTextBox = textBoxConverLastHotkey;
                    break;
                case HotKeyType.ConvertSelection:
                    currentTextBox = textBoxConvertSelectionHotkey;
                    break;
                case HotKeyType.SwitchLayout:
                    currentTextBox = textBoxSwitchLayoutHotkey;
                    break;
                default:
                    currentTextBox = null;
                    break;
            }
            if (currentTextBox == null) { return; }
            Invoke((MethodInvoker)delegate { currentTextBox.Text = ReplaceCtrls(text); });
        }

        private string ReplaceCtrls(string text)
        {
            return text
                .Replace("Control + LControlKey", "Left Ctrl")
                .Replace("Control + RControlKey", "Right Ctrl")
                .Replace("Shift + LShiftKey", "Left Shift")
                .Replace("Shift + RShiftKey", "Right Shift")
                .Replace("Alt + LMenu", "Left Alt")
                .Replace("Alt + RMenu", "Right Alt");
        }

        void ResetCurrentHotkey(bool clear)
        {
            switch (currentHotkeyType)
            {
                case HotKeyType.ConvertLast:
                    currentHotkey = clear ? null : settings.ConvertLastHotkey;
                    break;
                case HotKeyType.ConvertSelection:
                    currentHotkey = clear ? null : settings.ConvertSelectionHotkey;
                    break;
                case HotKeyType.SwitchLayout:
                    currentHotkey = clear ? null : settings.SwitchLayoutHotkey;
                    break;
                default:
                    currentHotkey = null;
                    break;
            }
            SetCurrentHotkeyInputText(currentHotkey == null ? "None" : ReplaceCtrls(currentHotkey.ToString()));
        }

        void ApplyCurrentHotkey()
        {
            if (!Visible || currentHotkey == null)
            {
                return;
            }
            switch (currentHotkeyType)
            {
                case HotKeyType.ConvertLast:
                    settings.ConvertLastHotkey = currentHotkey;
                    break;
                case HotKeyType.ConvertSelection:
                    settings.ConvertSelectionHotkey = currentHotkey;
                    break;
                case HotKeyType.SwitchLayout:
                    settings.SwitchLayoutHotkey = currentHotkey;
                    break;
                default:
                    break;
            }
            currentHotkeyType = HotKeyType.None;
        }

        /**
         * SETTINGS
         */
        void SaveSettings()
        {
            settings.Save();

            if (settings.AutoStart == true) { LowLevelAdapter.CreateAutorunShortcut(); }
            else { LowLevelAdapter.DeleteAutorunShortcut(); }
        }
        void ResetSettings()
        {
            settings.Reload();
        }

        /**
         * OTHER INPUTS
         */
        void checkBoxAutorun_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoStart = checkBoxAutorun.Checked;
        }
        private void checkBoxTrayIcon_CheckedChanged(object sender, EventArgs e)
        {
            settings.ShowTrayIcon = checkBoxTrayIcon.Checked;
        }
        void DisplaySwitchDelay(int delay)
        {
            textBoxDelay.Text = delay.ToString() + " ms";
        }
        private void textBoxDelay_TextChanged(object sender, EventArgs e)
        {
            short delay = 0;
            if (!Int16.TryParse(Regex.Replace(textBoxDelay.Text, "[^0-9]", ""), out delay)/* || delay < 1*/)
            {
                delay = 0;
            }
            settings.SwitchDelay = delay;
            DisplaySwitchDelay(delay);
        }

        /**
         * MISC
         */
        void OnExit()
        {
            engine.Stop();
            if (Exit != null)
            {
                Exit(this, null);
            }
        }
        private void buttonGithub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/vrd/dotSwitcher/issues");
        }

        private void label5_MouseHover(object sender, EventArgs e)
        {
            toolTip1.Show("Hold Ctrl or Shift button to assign Ctrl or Shift itself", textBoxSwitchLayoutHotkey);
        }

        private void label5_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(label5);
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void smartSelection_CheckedChanged(object sender, EventArgs e)
        {
            settings.SmartSelection = checkBoxSmartSelection.Checked;
        }

        private void autoSwitching_CheckedChanged(object sender, EventArgs e)
        {
            settings.AutoSwitching = checkBoxAutoSwitching.Checked;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
