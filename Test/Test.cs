using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using AutoMova.WinApi;
using AutoMova.Switcher;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoMovaTest
{
    [TestClass]
    public class AppTest
    {
        [TestMethod]
        public void Test()
        {
            AutoSwitchingTest(100);
        }

        private void AutoSwitchingTest(int testSetLength)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            Debug.WriteLine($"Current path is {path}");
            var app = StartApp(path);
            var notepad = StartNotepad();

            var layouts = LowLevelAdapter.GetLayoutList();

            var inputLangCollection = InputLanguage.InstalledInputLanguages;
            InputLanguage[] langs = new InputLanguage[layouts.Length];
            inputLangCollection.CopyTo(langs, 0);
            Dictionary<IntPtr, string[]> testStrings = new Dictionary<IntPtr, string[]>();
            foreach (var lang in langs)
            {
                testStrings.Add(layouts[Array.IndexOf(langs, lang)], System.IO.File.ReadAllLines($"{path}\\..\\..\\Dataset\\{lang.Culture.Name.Substring(0,2)}.txt"));
            }

            string expectedString = "";
            List<List<Keys>> testKeyCodes = new List<List<Keys>>();
            foreach (var layout in testStrings.Keys)
            {
                var testSet = testStrings[layout].Take(testSetLength).ToArray();
                foreach (var str in testSet)
                {
                    expectedString += (str + " ");
                    PressKeys(StringToKeys(str, layout));                    
                }
                LowLevelAdapter.SendKeyPress(Keys.Right);
            }

            LowLevelAdapter.SendSelectAll();
            LowLevelAdapter.SendCopy();
            LowLevelAdapter.SendKeyPress(Keys.Delete);
            notepad.CloseMainWindow();
            notepad.Close();
            app.Kill();
            var actualString = Clipboard.GetText();
            var expectedWords = expectedString.Split(' ');
            var actualWords = actualString.Split(' ');
            for (int i = 0; i < expectedWords.Length; i++)
            {
                if (expectedWords[i] != actualWords[i])
                {
                    Debug.WriteLine($"{expectedWords[i]} -> {actualWords[i]}");
                }

            }
            CollectionAssert.AreEqual(expectedWords, actualWords, "Auto switching error");            

        }

        private Process StartNotepad()
        {
            var notepad = Process.Start("notepad.exe");
            notepad.WaitForInputIdle();
            return notepad;
        }

        private Process StartApp(string path)
        {
            var app = Process.Start($"{path}\\..\\..\\..\\bin\\Release\\AutoMova.exe");
            app.WaitForInputIdle();
            return app;
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
                    // Increase sleep time if you run tests on potato and get inconsistent results
                    Thread.Sleep(10);
                }
            }
        }
    }
}
