namespace AutoMova.Data
{
    public interface ISettings
    {
        KeyboardEventArgs ConvertLastHotkey { get; set; }
        KeyboardEventArgs SwitchLayoutHotkey { get; set; }
        KeyboardEventArgs ConvertSelectionHotkey { get; set; }
        KeyboardEventArgs ToggleAutoSwitchingHotkey { get; set; }
        KeyboardEventArgs AddRemoveHotkey { get; set; }
        int SwitchDelay { get; set; }
        bool? SmartSelection { get; set; }
        bool? AutoSwitching { get; set; }
        bool? LegacySwitch { get; set; }
    }
}