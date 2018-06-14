using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotSwitcher.Switcher
{
    class LayoutDetector
    {
        private Dictionary<IntPtr, Dictionary> dictionaries = new Dictionary<IntPtr, Dictionary>();
        
        public LayoutDetector(IntPtr[] layouts)
        {
            foreach (var layout in layouts)
            {
                dictionaries.Add(layout, new Dictionary(ToLang(layout)));
            }
        }

        public IntPtr Decision(Dictionary<IntPtr, string> lastWord, IntPtr currentLayout)
        {
            foreach (var dict in dictionaries)
            {
                if (dict.Value.Contains(lastWord[dict.Key]))
                {
                    return dict.Key;
                }
            }
            return currentLayout;
        }

        private string ToLang(IntPtr layout)
        {
            switch (layout.ToInt32())
            {
                case 67699721: return "en";
                case 68748313: return "ru";
                case 69338146: return "uk";
                default: return null;
            }
        }
    }
}
