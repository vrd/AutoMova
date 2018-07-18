using AutoMova.Data;
using AutoMova.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AutoMova.Switcher
{
    public class SwitcherCore : IDisposable
    {
        public event EventHandler<SwitcherErrorArgs> Error;
        
        private KeyboardHook kbdHook;
        private MouseHook mouseHook;
        private ISettings settings;
        private bool readyToSwitch;
        private bool autoSwitchingIsGoing;
        private bool manualSwitchingIsGoing;
        private bool ignoreKeyPress;
        private Dictionary<string, uint> langToLayout = new Dictionary<string, uint>();
        private Dictionary<string, IntPtr> langToIntPtr = new Dictionary<string, IntPtr>();
        private Dictionary<IntPtr, string> layoutToLang = new Dictionary<IntPtr, string>();
        private LayoutDetector layoutDetector;
        private List<KeyboardEventArgs> currentSelection = new List<KeyboardEventArgs>();
        private Dictionary<string, string> lastWord = new Dictionary<string, string>();
        private Dictionary<string, int> langStatistics = new Dictionary<string, int>();


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
            ignoreKeyPress = false;
            
            var layouts = LowLevelAdapter.GetLayoutList();
            var inputLangCollection = InputLanguage.InstalledInputLanguages;
            InputLanguage[] inputLangs = new InputLanguage[layouts.Length];
            inputLangCollection.CopyTo(inputLangs, 0);
            foreach (var lang in inputLangs)
            {   
                Debug.WriteLine(lang.Culture.Name);                
                Debug.WriteLine((layouts[Array.IndexOf(inputLangs, lang)]).ToString("x8"));                
                if (!langToLayout.ContainsKey(lang.Culture.Name))
                {
                    langToLayout.Add(lang.Culture.Name, (uint)(layouts[Array.IndexOf(inputLangs, lang)]));
                    langToIntPtr.Add(lang.Culture.Name, layouts[Array.IndexOf(inputLangs, lang)]);
                }                
                layoutToLang.Add(layouts[Array.IndexOf(inputLangs, lang)], lang.Culture.Name);
            }
            var langs = langToLayout.Keys.ToArray();
            foreach (var lang in langs)
            {   
                lastWord.Add(lang, "");
                langStatistics.Add(lang, 0);
            }
            layoutDetector = new LayoutDetector(langs);
        }

        public static bool IsPrintable(KeyboardEventArgs evtData)
        {
            if (evtData.Alt || evtData.Control || evtData.Win) { return false; }
            var keyCode = evtData.KeyCode;
            if (keyCode >= Keys.D0 && keyCode <= Keys.Z) { return true; }
            if (keyCode >= Keys.Oem1 && keyCode <= Keys.OemBackslash) { return true; }
            if (keyCode >= Keys.NumPad0 && keyCode <= Keys.NumPad9) { return true; }
            if (keyCode == Keys.Decimal) { return true; }
            return false;
        }

        public static bool IsPunctuation(KeyboardEventArgs evtData, string lang)
        {
            switch (lang.Substring(0,2))
            {
                case "en":
                    {
                        return false;
                    }
                case "ru":
                case "uk":
                default:
                    {   // , .
                        if (evtData.KeyCode == Keys.OemQuestion ||
                                (evtData.Shift && 
                                    // !
                                    (evtData.KeyCode == Keys.D1 ||
                                     // ;
                                     evtData.KeyCode == Keys.D4 ||
                                     // :
                                     evtData.KeyCode == Keys.D6 ||
                                     // ?
                                     evtData.KeyCode == Keys.D7)))
                        {
                            return true;
                        }
                        return false;
                    }
            }
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
            if (ignoreKeyPress)
            {
                return;
            }

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
                ClearLangStatistics();
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
            Debug.WriteLine("OnKeyRelease: KeyCode=" + evtData.KeyCode.ToString("x"));
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
                Debug.WriteLine("ConvertLastHotkey detected!");
                manualSwitchingIsGoing = true;
                ConvertLast("next");
                manualSwitchingIsGoing = false;
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.ConvertSelectionHotkey))
            {
                Debug.WriteLine("ConvertSelectionHotkey detected!");
                ConvertSelection();
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.ToggleAutoSwitchingHotkey))
            {
                Debug.WriteLine("ToggleAutoSwitchingHotkey detected!");
                settings.AutoSwitching = !settings.AutoSwitching;
                evtData.Handled = true;
                return;
            }

            if (this.KeepTrackingKeys(evtData))
                return;
            
            var notModified = !this.HaveModifiers(evtData);

            if (vkCode == Keys.Back && notModified)
            {
                RemoveLast();
                return;
            }

            if (vkCode == Keys.Space && notModified)
            {                
                AddToCurrentSelection(evtData);
                return;                
            }

            if (IsPrintable(evtData))
            {
                var currentLayout = layoutToLang[LowLevelAdapter.GetCurrentLayout()];
                if (WordEnded())
                {
                    langStatistics[currentLayout] += 1;
                }
                if (settings.SmartSelection == false && GetPreviousVkCode() == Keys.Space)
                {
                    BeginNewSelection();
                }               
                AddToCurrentSelection(evtData);
                if (IsPunctuation(evtData, currentLayout))
                {
                    return;
                }
                if (!autoSwitchingIsGoing && !manualSwitchingIsGoing && currentSelection.Count > 1)
                {
                    var detectedLayout = layoutDetector.Decision(lastWord, SuggestedLang());
                    Debug.WriteLine($"Current layout: {currentLayout}, detected layout: {detectedLayout}");
                    if (settings.AutoSwitching == true && detectedLayout != currentLayout)
                    {                    
                        autoSwitchingIsGoing = true;                    
                         evtData.Handled = true;
                        //evtData.SuppressKeyPress = true;
                        //ConvertLast(CalculateSwitchingNumber(currentLayout, detectedLayout));
                        ConvertLast(detectedLayout);
                        autoSwitchingIsGoing = false;
                    }
                }
                
                return;                                
            }

            // default: 
            BeginNewSelection();
            ClearLangStatistics();
        }

        private void OnError(Exception ex)
        {
            if (Error != null)
            {
                Error(this, new SwitcherErrorArgs(ex));
            }
        }

        private Keys GetPreviousVkCode()
        {
            if (currentSelection.Count == 0) { return Keys.None; }
            return currentSelection[currentSelection.Count - 1].KeyCode;
        }

        private void ClearLangStatistics()
        {
            foreach (var lang in langStatistics.ToArray())
            {
                langStatistics[lang.Key] = 0;
            }
        }

        private string SuggestedLang()
        {
            return langStatistics.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
        }

        private bool WordEnded()
        {
            return (currentSelection.Count >= 2) &&
                (currentSelection[currentSelection.Count - 1].KeyCode == Keys.Space) &&
                (currentSelection[currentSelection.Count - 2].KeyCode != Keys.Space) &&
                (IsPrintable(currentSelection[currentSelection.Count - 2]));
        }

        private void BeginNewSelection()
        {
            Debug.Write("BeginNewSelection()    ");
            currentSelection.Clear();
            foreach(var word in lastWord.ToArray())
            {
                lastWord[word.Key] = "";
            }
            Debug.WriteLine(DictToString(lastWord));
        }

        private void AddToCurrentSelection(KeyboardEventArgs data)
        {
            Debug.Write("AddToCurrentSelection()    ");
            currentSelection.Add(data);
            foreach (var word in lastWord.ToArray())
            {
                lastWord[word.Key] = word.Value + LowLevelAdapter.KeyCodeToUnicode(data.KeyCode, langToIntPtr[word.Key]);
            }
            Debug.WriteLine(DictToString(lastWord));
        }

        private void RemoveLast()
        {
            Debug.WriteLine("RemoveLast()   ");
            if (currentSelection.Count == 0) { return; }
            currentSelection.RemoveAt(currentSelection.Count - 1);
            foreach (var word in lastWord.ToArray())
            {
                lastWord[word.Key] = word.Value.Substring(0, word.Value.Length - 1);
            }
            //Debug.WriteLine(DictToString(lastWord));
        }        

        private string DictToString(Dictionary<string, string> dictionary)
        {
            var str = "";
            foreach (var item in dictionary)
            {
                str += item.Key + ": '" + item.Value + "'; ";
            }
            return str;
        }  
        
        //private int CalculateSwitchingNumber(IntPtr currentLayout, IntPtr detectedLayout)
        //{
        //    var switchingNumber = Array.IndexOf(layouts, detectedLayout) - Array.IndexOf(layouts, currentLayout);
        //    if (switchingNumber < 0)
        //    {
        //        switchingNumber = switchingNumber + layouts.Length;
        //    }
        //    return switchingNumber;
        //}

        private void RemoveSelection()
        {
            ignoreKeyPress = true;
            LowLevelAdapter.SendKeyPress(Keys.Space);
            LowLevelAdapter.SendKeyPress(Keys.Back);
            ignoreKeyPress = false;
        }

        private void ConvertSelection()
        {
            ignoreKeyPress = true;

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

            ignoreKeyPress = false;
        }

        private void SwitchLayout()
        {
            BeginNewSelection();
            ignoreKeyPress = true;
            LowLevelAdapter.ReleasePressedFnKeys();
            LowLevelAdapter.SetNextKeyboardLayout();
            ignoreKeyPress = false;
        }

        private void ConvertLast(string lang)
        {
            Debug.WriteLine($"ConvertLast to {lang}...");
            var fnKeys = LowLevelAdapter.ReleasePressedFnKeys();
            var selection = currentSelection.ToList();
            BeginNewSelection();
            // Fix for apps with autocompletion (i.e. omnibox in Google Chrome browser)
            RemoveSelection();
            // Remove last word
            var backspaceCount = autoSwitchingIsGoing ? (selection.Count - 1) : selection.Count;
            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, backspaceCount);
            foreach (var vkCode in backspaces)
            {
                Thread.Sleep(settings.SwitchDelay);
                LowLevelAdapter.SendKeyPress(vkCode, false);
            }
            
            ignoreKeyPress = true;
            
            if (lang == "next")
            {
                LowLevelAdapter.SetNextKeyboardLayout();
            }
            else
            {
                LowLevelAdapter.SetKeyboadLayout(langToLayout[lang]);
            }
            
            ignoreKeyPress = false;

            // Type last word in new layout
            foreach (var data in selection)
            {
                Thread.Sleep(settings.SwitchDelay);
                LowLevelAdapter.SendKeyPress(data.KeyCode, data.Shift);
            }

            LowLevelAdapter.PressPressedFnKeys(fnKeys);
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
