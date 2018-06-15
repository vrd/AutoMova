namespace AutoSwitcher.Data
{
    public interface ISettings
    {
        KeyboardEventArgs ConvertLastHotkey { get; set; }
        KeyboardEventArgs SwitchLayoutHotkey { get; set; }
        KeyboardEventArgs ConvertSelectionHotkey { get; set; }
        int SwitchDelay { get; set; }
        bool? SmartSelection { get; set; }
        bool? AutoSwitching { get; set; }
    }
}