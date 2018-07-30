using System;
using System.Configuration;
using System.Windows.Forms;
using AutoMova.WinApi;

namespace AutoMova.Data
{
    [Serializable]
    public sealed class Settings : ApplicationSettingsBase, ISettings
    {
        public event EventHandler<AutoToggleArgs> AutoSwitchingToggle;

        public static Settings Init()
        {
            var settings = new Settings();
            settings.Reload();
            bool laptop = LowLevelAdapter.ThisIsLaptop();
            if (settings.ConvertLastHotkey.KeyData == Keys.None)
            {
                settings.ConvertLastHotkey = new KeyboardEventArgs(laptop ? Keys.End : Keys.Pause, false);
            }
            if (settings.ConvertSelectionHotkey.KeyData == Keys.None)
            {
                settings.ConvertSelectionHotkey = new KeyboardEventArgs(laptop ? (Keys.End | Keys.Shift) : (Keys.Pause | Keys.Shift), false);
            }
            if (settings.ToggleAutoSwitchingHotkey.KeyData == Keys.None)
            {
                settings.ToggleAutoSwitchingHotkey = new KeyboardEventArgs(laptop ? (Keys.End | Keys.Control) : (Keys.Cancel | Keys.Control), false);
            }
            if (settings.AddRemoveHotkey.KeyData == Keys.None)
            {
                settings.AddRemoveHotkey = new KeyboardEventArgs(laptop ? (Keys.End | Keys.Alt) : (Keys.Pause | Keys.Alt), false);
            }
            if (settings.ShowTrayIcon == null)
            {
                settings.ShowTrayIcon = true;
            }
            if (settings.SmartSelection == null)
            {
                settings.SmartSelection = false;
            }
            if (settings.AutoSwitching == null)
            {
                settings.AutoSwitching = true;
            }
            if (settings.SwitchDelay < 0)
            {
                settings.SwitchDelay = 0;
            }
            settings.Save();
            return settings;
        }

        private void OnAutoSwitchingModeToggle(bool value)
        {
            if (AutoSwitchingToggle != null)
            {
                AutoSwitchingToggle(this, new AutoToggleArgs(value));
            }
        }       

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs ConvertLastHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["ConvertLastHotkey"];
            }
            set
            {
                this["ConvertLastHotkey"] = (KeyboardEventArgs)value; 
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs ConvertSelectionHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["ConvertSelectionHotkey"];
            }
            set
            {
                this["ConvertSelectionHotkey"] = (KeyboardEventArgs)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs SwitchLayoutHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["SwitchLayoutHotkey"];
            }
            set
            {
                this["SwitchLayoutHotkey"] = (KeyboardEventArgs)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs ToggleAutoSwitchingHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["ToggleAutoSwitchingHotkey"];
            }
            set
            {
                this["ToggleAutoSwitchingHotkey"] = (KeyboardEventArgs)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public KeyboardEventArgs AddRemoveHotkey
        {
            get
            {
                return (KeyboardEventArgs)this["AddRemoveHotkey"];
            }
            set
            {
                this["AddRemoveHotkey"] = (KeyboardEventArgs)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool? AutoStart
        {
            get
            {
                return (bool?)this["AutoStart"];
            }
            set
            {
                this["AutoStart"] = (bool?)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool? ShowTrayIcon
        {
            get
            {
                return (bool?)this["ShowTrayIcon"];
            }
            set
            {
                this["ShowTrayIcon"] = (bool?)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public int SwitchDelay
        {
            get
            {
                return (int)this["SwitchDelay"];
            }
            set
            {
                this["SwitchDelay"] = (int)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool? SmartSelection
        {
            get
            {
                return (bool?)this["SmartSelection"];
            }
            set
            {
                this["SmartSelection"] = (bool?)value;
            }
        }

        [UserScopedSetting]
        [SettingsSerializeAs(SettingsSerializeAs.Binary)]
        [DefaultSettingValue("")]
        public bool? AutoSwitching
        {
            get
            {
                return (bool?)this["AutoSwitching"];
            }
            set
            {
                this["AutoSwitching"] = (bool?)value;
                if (value.HasValue)
                {
                    OnAutoSwitchingModeToggle((bool)value);
                }
                else
                {
                    OnAutoSwitchingModeToggle(false);
                }
                
            }
        }
    }

    public class AutoToggleArgs : EventArgs
    {
        public bool newValue;
        public AutoToggleArgs(bool value)
        {
            newValue = value;
        }
    }

}
