using AutoSwitcher.Data;
using AutoSwitcher.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoSwitcher.Switcher
{
    public class SwitcherCore : IDisposable
    {
        public event EventHandler<SwitcherErrorArgs> Error;
        private List<KeyboardEventArgs> currentSelection = new List<KeyboardEventArgs>();
        private Dictionary<IntPtr, string> lastWord = new Dictionary<IntPtr, string>();
        private KeyboardHook kbdHook;
        private MouseHook mouseHook;
        private ISettings settings;
        private bool readyToSwitch;
        private bool autoSwitchingIsGoing;
        private bool manualSwitchingIsGoing;
        private IntPtr[] layoutList;
        private LayoutDetector layoutDetector;

        public SwitcherCore(ISettings settings)
        {
            this.settings = settings;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += ProcessKeyPress;
            mouseHook = new MouseHook();
            mouseHook.MouseEvent += ProcessMousePress;
            readyToSwitch = false;
            autoSwitchingIsGoing = false;
            manualSwitchingIsGoing = false;
            layoutList = LowLevelAdapter.GetLayoutList();
            foreach (var layout in layoutList)
            {
                lastWord.Add(layout, "");
            }
            layoutDetector = new LayoutDetector(layoutList);
        }

        public static bool IsPrintable(KeyboardEventArgs evtData)
        {
            if (evtData.Alt || evtData.Control || evtData.Win) { return false; }
            var keyCode = evtData.KeyCode;
            if (keyCode >= Keys.D0 && keyCode <= Keys.Z) { return true; }
            if (keyCode >= Keys.Oem1 && keyCode <= Keys.OemBackslash) { return true; }
            if (keyCode >= Keys.NumPad0 && keyCode <=Keys.NumPad9) { return true; }
            if (keyCode == Keys.Decimal) { return true; }
            return false;
        }

        public bool IsStarted()
        {
            return kbdHook.IsStarted() || mouseHook.IsStarted();
        }
        public void Start()
        {
            kbdHook.Start();
            mouseHook.Start();
        }
        public void Stop()
        {
            kbdHook.Stop();
            mouseHook.Stop();
        }

        // evtData.Handled must be set to true if the key is recognized as hotkey and doesn't need further processing
        private void ProcessKeyPress(object sender, KeyboardEventArgs evtData)
        {
            try
            {
                if (evtData.Type == KeyboardEventType.KeyDown)
                    OnKeyPress(evtData);
                else
                    OnKeyRelease(evtData);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void ProcessMousePress(object sender, EventArgs evtData)
        {
            try
            {
                BeginNewSelection();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private bool HaveModifiers(KeyboardEventArgs evtData)
        {
            var ctrl = evtData.Control;
            var alt = evtData.Alt;
            var win = evtData.Win;

            return ctrl || alt || win;
        }

        private bool KeepTrackingKeys(KeyboardEventArgs evtData)
        {
            var vkCode = evtData.KeyCode;

            return vkCode == Keys.ControlKey ||
              vkCode == Keys.LControlKey ||
              vkCode == Keys.RControlKey ||
                // yes, don't interrupt the tracking on PrtSc!
              vkCode == Keys.PrintScreen ||
              vkCode == Keys.ShiftKey ||
              vkCode == Keys.RShiftKey ||
              vkCode == Keys.LShiftKey ||
              vkCode == Keys.NumLock ||
              vkCode == Keys.Scroll;
        }

        private void OnKeyRelease(KeyboardEventArgs evtData)
        {
            if (evtData.Equals(settings.SwitchLayoutHotkey) && readyToSwitch)
            {
                SwitchLayout();
                evtData.Handled = false;
                return;
            }
        }

        private void OnKeyPress(KeyboardEventArgs evtData)
        {
            var vkCode = evtData.KeyCode;
            Debug.WriteLine("OnKeyPress: KeyCode="+vkCode.ToString("x"));

            if (evtData.Equals(settings.SwitchLayoutHotkey))
            {
                readyToSwitch = true;
                return;
            }

            readyToSwitch = false;

            if (evtData.Equals(settings.ConvertLastHotkey))
            {
                manualSwitchingIsGoing = true;
                ConvertLast(1);
                manualSwitchingIsGoing = false;
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.ConvertSelectionHotkey))
            {
                ConvertSelection();
            }

            if (this.KeepTrackingKeys(evtData))
                return;
            
            var notModified = !this.HaveModifiers(evtData);

            if (vkCode == Keys.Back && notModified) { RemoveLast(); return; }

            if (vkCode == Keys.Space && notModified)
            {
                if (settings.SmartSelection == false)
                {
                    BeginNewSelection();
                    return;
                }
                else
                {
                    AddToCurrentSelection(evtData);
                    return;
                }
            }

            if (IsPrintable(evtData))
            {
                var currentLayout = LowLevelAdapter.GetCurrentLayout();
                AddToCurrentSelection(evtData);
                //Debug.WriteLine(LowLevelAdapter.KeyCodeToUnicode(vkCode, currentLayout));

                var detectedLayout = layoutDetector.Decision(lastWord, currentLayout);                

                if (settings.AutoSwitching == true && detectedLayout != currentLayout && !autoSwitchingIsGoing && !manualSwitchingIsGoing)
                {                    
                    autoSwitchingIsGoing = true;
                    evtData.Handled = true;
                    evtData.SuppressKeyPress = true;
                    ConvertLast(CalculateSwitchingNumber(currentLayout, detectedLayout));                    
                    autoSwitchingIsGoing = false;
                }
                return;                                
            }

            // default: 
            BeginNewSelection();
        }

        private void OnError(Exception ex)
        {
            if (Error != null)
            {
                Error(this, new SwitcherErrorArgs(ex));
            }
        }

        #region selection manipulation
        private Keys GetPreviousVkCode()
        {
            if (currentSelection.Count == 0) { return Keys.None; }
            return currentSelection[currentSelection.Count - 1].KeyCode;
        }

        private void BeginNewSelection()
        {
            Debug.WriteLine("BeginNewSelection()...");
            currentSelection.Clear();
            foreach(var word in lastWord.ToArray())
            {
                lastWord[word.Key] = "";
            }
            Debug.WriteLine(DictToString(lastWord));
        }

        private void AddToCurrentSelection(KeyboardEventArgs data)
        {
            Debug.WriteLine("AddToCurrentSelection()...");
            currentSelection.Add(data);
            foreach (var word in lastWord.ToArray())
            {
                lastWord[word.Key] = word.Value + LowLevelAdapter.KeyCodeToUnicode(data.KeyCode, word.Key);
            }
            Debug.WriteLine(DictToString(lastWord));
        }

        private void RemoveLast()
        {
            Debug.WriteLine("RemoveLast()...");
            if (currentSelection.Count == 0) { return; }
            currentSelection.RemoveAt(currentSelection.Count - 1);
            foreach (var word in lastWord.ToArray())
            {
                lastWord[word.Key] = word.Value.Substring(0, word.Value.Length - 1);
            }
            Debug.WriteLine(DictToString(lastWord));
        }
        #endregion

        private string DictToString(Dictionary<IntPtr, string> dictionary)
        {
            var str = "";
            foreach (var item in dictionary)
            {
                str += item.Key.ToString() + ": '" + item.Value + "'; ";
            }
            return str;
        }

        private int CalculateSwitchingNumber(IntPtr currentLayout, IntPtr detectedLayout)
        {
            //FIXME: remove this shitcode 
             //     case 67699721: return "en";
             //     case 68748313: return "ru";
             //     case 69338146: return "uk";
            var oldl = currentLayout.ToInt32();
            var newl = detectedLayout.ToInt32();
            if ((oldl == 67699721 && newl == 68748313) ||
                (oldl == 68748313 && newl == 69338146) ||
                (oldl == 69338146 && newl == 67699721))
            {
                return 1;
            }
            return 2;
        }

        private void ConvertSelection()
        {
            LowLevelAdapter.BackupClipboard();
            LowLevelAdapter.SendCopy();
            var selection = Clipboard.GetText();
            LowLevelAdapter.RestoreClipboard();
            if (String.IsNullOrEmpty(selection))
            {
                return;
            }
            
            LowLevelAdapter.ReleasePressedFnKeys();

            var keys = new List<Keys>(selection.Length);
            for(var i = 0; i < selection.Length; i++)
            {
                keys.Add(LowLevelAdapter.ToKey(selection[i]));
            }

            LowLevelAdapter.SetNextKeyboardLayout();

            foreach (var key in keys)
            {
                Debug.Write(key);
                if (key != Keys.None)
                {
                    LowLevelAdapter.SendKeyPress(key, (key & Keys.Shift) != Keys.None);
                }
            }

        }

        private void SwitchLayout()
        {
            BeginNewSelection();
            LowLevelAdapter.SetNextKeyboardLayout();
        }

        private void ConvertLast(int switchingNumber)
        {
            Debug.WriteLine("ConvertLast()...");
            LowLevelAdapter.ReleasePressedFnKeys();
            var selection = currentSelection.ToList();
            BeginNewSelection();
            // Remove last word
            var backspaceCount = autoSwitchingIsGoing ? (selection.Count-1) : selection.Count;
            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, backspaceCount);
            foreach (var vkCode in backspaces)
            {
                LowLevelAdapter.SendKeyPress(vkCode, false);
            }
            // Fix for skype
            Thread.Sleep(settings.SwitchDelay);
            // Switch layout proper number of times
            for (int i = 0; i < switchingNumber; i++)
            {
                LowLevelAdapter.SetNextKeyboardLayout();
            }
            // Type last word in new layout
            foreach (var data in selection)
            {
                LowLevelAdapter.SendKeyPress(data.KeyCode, data.Shift);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class SwitcherErrorArgs : EventArgs
    {
        public Exception Error { get; private set; }
        public SwitcherErrorArgs(Exception ex)
        {
            Error = ex;
        }
    }

}
