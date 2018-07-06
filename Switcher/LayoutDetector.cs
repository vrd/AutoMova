using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NHunspell;

namespace AutoMova.Switcher
{
    class LayoutDetector
    {   
        //TODO: user dictionaries
        private Dictionary<IntPtr, UserDictionary> userDictionaries = new Dictionary<IntPtr, UserDictionary>();
        private Dictionary<IntPtr, Hunspell> hunspellDictionaries = new Dictionary<IntPtr, Hunspell>();
        private Dictionary<IntPtr, ProtoDictionary> protoDictionaries = new Dictionary<IntPtr, ProtoDictionary>();
        
        public LayoutDetector(IntPtr[] layouts)
        {
            Debug.WriteLine($"Current path is {AppDomain.CurrentDomain.BaseDirectory}");
            foreach (var layout in layouts)
            {   //Load user dictionaries
                userDictionaries.Add(layout, new UserDictionary(ToLangCode(layout)));

                //Load Hunspell dictionaries
                var hunspellPath = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\hunspell\\" + ToLangCode(layout) + "\\" + ToLangCountryCode(layout);
                var affFile = hunspellPath + ".aff";
                var dicFile = hunspellPath + ".dic";
                hunspellDictionaries.Add(layout, new Hunspell(affFile, dicFile));

                //Load proto-dictionaries
                protoDictionaries.Add(layout, new ProtoDictionary(ToLangCode(layout)));
            }
        }

        public IntPtr Decision(Dictionary<IntPtr, string> lastWord, IntPtr currentLayout)
        {
            foreach (var dict in userDictionaries)
            {
                if (dict.Value.Contains(lastWord[dict.Key].Trim()))
                {
                    Debug.WriteLine($"Word found in user {ToLangCode(dict.Key).ToUpper()}");
                    return dict.Key;
                }
            }

            foreach (var dict in hunspellDictionaries)
            {
                if (dict.Value.Spell(lastWord[dict.Key].Trim()))
                {
                    Debug.WriteLine($"Word found in Hunspell {ToLangCode(dict.Key).ToUpper()}");
                    return dict.Key;
                }
            }

            foreach (var dict in protoDictionaries)
            {
                if (dict.Value.Contains(lastWord[dict.Key].Trim()))
                {
                    Debug.WriteLine($"Word found in proto {ToLangCode(dict.Key).ToUpper()}");
                    return dict.Key;
                }
            }

            Debug.WriteLine($"Word not found anywhere");
            return currentLayout;
        }

        public string ToLangCode(IntPtr layout)
        {
            switch (((UInt16)layout).ToString("x4"))
            {
                case "0409": return "en";
                case "0419": return "ru";
                case "0422": return "uk";
                default: return null;            
            }
        }

        private string ToLangCountryCode(IntPtr layout)
        {
            switch (((UInt16)layout).ToString("x4"))
            {
                case "0409": return "en_US";
                case "0419": return "ru_RU";
                case "0422": return "uk_UA";
                default: return null;
            }
        }
    }
}
