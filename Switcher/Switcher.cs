using AutoMova.Data;
using AutoMova.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace AutoMova.Switcher
{
    public class SwitcherCore : IDisposable
    {
        public event EventHandler<SwitcherErrorArgs> Error;
        public event EventHandler<SwitcherInfoArgs> Info;

        private KeyboardHook kbdHook;
        private MouseHook mouseHook;
        private ISettings settings;
        private bool readyToSwitch;
        private bool autoSwitchingIsGoing;
        private bool manualSwitchingIsGoing;
        private bool ignoreKeyPress;
        private bool disableAutoSwitchingCurrentWord;
        private List<string> langsList = new List<string>();
        private int langsNumber = 0;
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
            disableAutoSwitchingCurrentWord = false;
            
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
                    langsList.Add(lang.Culture.Name);
                    langsNumber++;
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
                    {       // ; :
                        if (evtData.KeyCode == Keys.Oem1 ||
                           (!evtData.Shift &&
                            // .
                                (evtData.KeyCode == Keys.OemPeriod ||
                            // ,
                                evtData.KeyCode == Keys.Oemcomma)) ||                                   
                           (evtData.Shift &&
                            // ?
                                (evtData.KeyCode == Keys.OemQuestion ||
                            // "
                                evtData.KeyCode == Keys.Oem7 ||
                            // !
                                evtData.KeyCode == Keys.D1 ||
                            // (
                                evtData.KeyCode == Keys.D9 ||
                            // )
                                evtData.KeyCode == Keys.D0
                            )))                                 
                        {
                            return true;                            
                        }
                        return false;
                    }
                case "ru":
                case "uk":                
                    {   // , .
                        if (evtData.KeyCode == Keys.OemQuestion ||
                           (evtData.Shift && 
                                // !
                                (evtData.KeyCode == Keys.D1 ||
                                // "
                                evtData.KeyCode == Keys.D2 ||
                                // ;
                                evtData.KeyCode == Keys.D4 ||
                                // :
                                evtData.KeyCode == Keys.D6 ||
                                // ?
                                evtData.KeyCode == Keys.D7 ||
                                // (
                                evtData.KeyCode == Keys.D9 ||
                                // )
                                evtData.KeyCode == Keys.D0
                                )))
                        {
                            return true;
                        }
                        return false;
                    }
                default: return false;
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
              vkCode == Keys.RMenu ||
              vkCode == Keys.LMenu ||
              vkCode == Keys.NumLock ||
              vkCode == Keys.Scroll;
        }

        private void OnKeyRelease(KeyboardEventArgs evtData)
        {
            Debug.WriteLine($"OnKeyRelease: KeyCode={evtData.KeyCode.ToString("x")}, KeyData={evtData.KeyData.ToString("x")}, KeyData={evtData.KeyData.ToString()}");
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
            Debug.WriteLine($"\nOnKeyPress: KeyCode={vkCode.ToString("x")}, KeyData={evtData.KeyData.ToString("x")}, KeyData={evtData.KeyData.ToString()}");
            //return;
            if (evtData.Equals(settings.SwitchLayoutHotkey))
            {
                readyToSwitch = true;
                return;
            }

            readyToSwitch = false;

            if (evtData.Equals(settings.ConvertLastHotkey))
            {
                Debug.WriteLine("ConvertLastHotkey detected!");
                disableAutoSwitchingCurrentWord = true;
                manualSwitchingIsGoing = true;
                ConvertLast(null, "next");
                manualSwitchingIsGoing = false;
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.ConvertSelectionHotkey))
            {
                Debug.WriteLine("ConvertSelectionHotkey detected!");
                ConvertSelectionToNextLayout();
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

            if (evtData.Equals(settings.AddRemoveHotkey))
            {
                Debug.WriteLine("AddRemoveHotkey detected!");
                ChangeUserDict();
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

            if (IsPrintable(evtData) || (vkCode == Keys.Space && notModified))
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
                if (!autoSwitchingIsGoing && !manualSwitchingIsGoing && !disableAutoSwitchingCurrentWord && currentSelection.Count > 1)
                {
                    var suggestedLayout = SuggestedLang();
                    bool firstWord = false;
                    if (suggestedLayout == null)
                    {
                        suggestedLayout = currentLayout;
                        firstWord = true;
                    }
                    var detectedLayout = layoutDetector.DetectLayout(lastWord, suggestedLayout, firstWord);
                    Debug.WriteLine($"Current layout: {currentLayout}, detected layout: {detectedLayout}");
                    if (settings.AutoSwitching == true && detectedLayout != currentLayout)
                    {                    
                        autoSwitchingIsGoing = true;                    
                        evtData.Handled = true;
                        ConvertLast(currentLayout, detectedLayout);
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
            Error(this, new SwitcherErrorArgs(ex));
        }

        private void EmitInfo(string word, string lang, bool success, bool add)
        {            
            Info(this, new SwitcherInfoArgs(word, lang, success, add));
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
            Debug.WriteLine($"Lang stat: {DictToString(langStatistics)}");
            if (langStatistics.Sum(l => l.Value) == 0) return null;
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
            disableAutoSwitchingCurrentWord = false;
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

        private string DictToString(Dictionary<string, int> dictionary)
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

        private void ConvertSelectionToNextLayout()
        {
            ignoreKeyPress = true;

            var fnKeys = LowLevelAdapter.ReleasePressedFnKeys();
            var selection = LowLevelAdapter.GetSelectedText();
            if (String.IsNullOrEmpty(selection))
            {
                LowLevelAdapter.PressPressedFnKeys(fnKeys);
                ignoreKeyPress = false;
                Thread.Sleep(settings.SwitchDelay);
                return;
            }
            
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
                    Thread.Sleep(settings.SwitchDelay);
                }
            }
            
            foreach (var key in keys)
            {             
                LowLevelAdapter.SendKeyPress(Keys.Left, true);
                Thread.Sleep(settings.SwitchDelay);
            }

            Debug.WriteLine("");

            LowLevelAdapter.PressPressedFnKeys(fnKeys);
            Thread.Sleep(settings.SwitchDelay);

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

        private void ConvertLast(string fromLang, string toLang)
        {
            Debug.WriteLine($"ConvertLast to {toLang}...");
            var fnKeys = LowLevelAdapter.ReleasePressedFnKeys();
                        
            // Fix for apps with autocompletion (i.e. omnibox in Google Chrome browser)
            RemoveSelection();
            
            ignoreKeyPress = true;

            // Remove last word
            var backspaceCount = autoSwitchingIsGoing ? (currentSelection.Count - 1) : currentSelection.Count;
            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, backspaceCount);
            foreach (var backspace in backspaces)
            {                
                LowLevelAdapter.SendKeyPress(backspace, false);
                Thread.Sleep(settings.SwitchDelay);
            }           
            
            //Change layout
            if (toLang == "next")
            {
                if (settings.LegacySwitch == true)
                {
                    LowLevelAdapter.SetNextKeyboardLayoutByKeypress();
                }
                else
                {
                    LowLevelAdapter.SetNextKeyboardLayout();
                }
            }
            else
            {
                if (settings.LegacySwitch == true)
                {
                    var fromIndex = langsList.IndexOf(fromLang);
                    var toIndex = langsList.IndexOf(toLang);
                    var switchNum = toIndex - fromIndex;
                    if (switchNum < 0)
                    {
                        switchNum += langsNumber;                        
                    }
                    for (var i = 0; i < switchNum; i++) 
                    {
                        LowLevelAdapter.SetNextKeyboardLayoutByKeypress();
                    }                    
                }
                else
                {
                    LowLevelAdapter.SetKeyboadLayout(langToLayout[toLang]);
                }
                
            }
                       
            // Type last word in new layout
            foreach (var data in currentSelection)
            {               
                LowLevelAdapter.SendKeyPress(data.KeyCode, data.Shift);
                Thread.Sleep(settings.SwitchDelay);
            }

            LowLevelAdapter.PressPressedFnKeys(fnKeys);

            ignoreKeyPress = false;
        }

        private void ChangeUserDict()
        {
            string lang = layoutToLang[LowLevelAdapter.GetCurrentLayout()];
            string word;
            // get word from last typed symbols
            if (currentSelection.Count > 0)
            {                
                word = lastWord[lang].Trim();                
            }
            //get word from selection
            else
            {
                //ignoreKeyPress = true;                
                //var fnKeys = LowLevelAdapter.ReleasePressedFnKeys();                
                word = LowLevelAdapter.GetSelectedText();               
                //LowLevelAdapter.PressPressedFnKeys(fnKeys);
                //ignoreKeyPress = false;
            }
            if (IsWord(word))
            {
                Debug.WriteLine($"Word is {word}");
                var add = layoutDetector.AddOrRemove(word, lang);
                Debug.WriteLine("Word was " + (add ? "added" : "removed"));
                EmitInfo(word, lang, true, add);
            }
            else
            {
                Debug.WriteLine($"Word {word} is invalid");
                EmitInfo("", lang, false, true);
            }           
        }

        private bool IsWord(string symbols)
        {
            return !String.IsNullOrEmpty(symbols) && Regex.IsMatch(symbols, @"^\S+$");
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

    public class SwitcherInfoArgs : EventArgs
    {
        public string Info { get; private set; }
        public bool Success { get; private set; }
        public SwitcherInfoArgs(string word, string lang, bool success, bool add)
        {
            var not = success ? "" : "not ";
            var added = add ? "added to " : "removed from";
            Info = string.Format($"Word \"{word}\" {not}{added} dictionary {lang.Substring(0,2).ToUpper()}");
            Success = success;
        }
    }
}
