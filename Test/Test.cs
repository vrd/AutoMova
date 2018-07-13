using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using AutoMova.WinApi;
using AutoMova.Switcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMovaTest
{
    [TestClass]
    public class AppTest
    {
        [TestMethod]
        public void AutoSwitchingTest()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Debug.WriteLine($"Current path is {path}");
            var app = Process.Start($"{path}\\..\\Release\\AutoMova.exe");
            app.WaitForInputIdle();            
            var notepad = Process.Start("notepad.exe");
            notepad.WaitForInputIdle();

            var layouts = LowLevelAdapter.GetLayoutList();

            var inputLangCollection = InputLanguage.InstalledInputLanguages;
            InputLanguage[] langs = new InputLanguage[layouts.Length];
            inputLangCollection.CopyTo(langs, 0);
            Dictionary<IntPtr, string[]> testStrings = new Dictionary<IntPtr, string[]>();
            foreach (var lang in langs)
            {
                testStrings.Add(layouts[Array.IndexOf(langs, lang)], System.IO.File.ReadAllLines($"{path}\\..\\..\\Test\\data\\{lang.Culture.Name.Substring(0,2)}.txt"));
            }

            string expectedString = "";
            List<List<Keys>> testKeyCodes = new List<List<Keys>>();
            foreach (var layout in testStrings.Keys)
            {
                foreach (var str in testStrings[layout])
                {
                    expectedString += (str + " ");
                    PressKeys(StringToKeys(str, layout));                    
                }                
            }

            LowLevelAdapter.SendSelectAll();
            LowLevelAdapter.SendCopy();
            LowLevelAdapter.SendKeyPress(Keys.Delete);
            notepad.CloseMainWindow();
            notepad.Close();
            //app.CloseMainWindow();
            app.Kill();
            var actualString = Clipboard.GetText();
            var expectedWords = expectedString.Split(' ');
            var actualWords = actualString.Split(' ');
            for (int i = 0; i < expectedWords.Length; i++)
            {
                Assert.AreEqual(expectedWords[i], actualWords[i], false, "Auto switching error");
            }
            
        }

        private List<Keys> StringToKeys(string str, IntPtr layout)
        {
            var keys = new List<Keys>(str.Length + 1);
            for (var i = 0; i < str.Length; i++)
            {
                keys.Add(LowLevelAdapter.ToKey(str[i], layout));
            }
            keys.Add(Keys.Space);
            return keys;
        }

        private void PressKeys(List<Keys> keys)
        {
            foreach (var key in keys)
            {
                //Debug.Write(key);
                if (key != Keys.None)
                {
                    LowLevelAdapter.SendKeyPress(key, (key & Keys.Shift) != Keys.None);
                    Thread.Sleep(10);
                }
            }
        }
    }
}
