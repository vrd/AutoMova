using System;
using System.Configuration;
using System.Windows.Forms;

namespace AutoMova.Data
{
    [Serializable]
    public sealed class Settings : ApplicationSettingsBase, ISettings
    {
        public static Settings Init()
        {
            var settings = new Settings();
            settings.Reload();
            if (settings.ConvertLastHotkey.KeyData == Keys.None)
            {
                settings.ConvertLastHotkey = new KeyboardEventArgs(Keys.Pause, false);
            }
            if (settings.ConvertSelectionHotkey.KeyData == Keys.None)
            {
                settings.ConvertSelectionHotkey = new KeyboardEventArgs(Keys.Pause | Keys.Shift, false);
            }
            if (settings.ShowTrayIcon == null)
            {
                settings.ShowTrayIcon = true;
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
            }
        }
    }
    
}
