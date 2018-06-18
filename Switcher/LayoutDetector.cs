using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHunspell;

namespace AutoSwitcher.Switcher
{
    class LayoutDetector
    {   
        //TODO: user dictionaries
        //private Dictionary<IntPtr, Dictionary> userDictionaries = new Dictionary<IntPtr, Dictionary>();
        private Dictionary<IntPtr, Hunspell> hunspellDictionaries = new Dictionary<IntPtr, Hunspell>();
        private Dictionary<IntPtr, ProtoDictionary> protoDictionaries = new Dictionary<IntPtr, ProtoDictionary>();
        
        public LayoutDetector(IntPtr[] layouts)
        {
            foreach (var layout in layouts)
            {   
                //Load Hunspell dictionaries
                var hunspellPath = "hunspell\\" + ToLangCode(layout) + "\\" + ToLangCountryCode(layout);
                var affFile = hunspellPath + ".aff";
                var dicFile = hunspellPath + ".dic";
                hunspellDictionaries.Add(layout, new Hunspell(affFile, dicFile));

                //Load proto-dictionaries
                protoDictionaries.Add(layout, new ProtoDictionary(ToLangCode(layout)));
            }
        }

        public IntPtr Decision(Dictionary<IntPtr, string> lastWord, IntPtr currentLayout)
        {
            foreach (var dict in hunspellDictionaries)
            {
                if (dict.Value.Spell(lastWord[dict.Key]))
                {
                    return dict.Key;
                }
            }

            foreach (var dict in protoDictionaries)
            {
                if (dict.Value.Contains(lastWord[dict.Key]))
                {
                    return dict.Key;
                }
            }

            return currentLayout;
        }

        private string ToLangCode(IntPtr layout)
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
