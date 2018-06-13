using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotSwitcher.Switcher
{
    class LanguageDetector
    {
        private Dictionary englishDict;
        private Dictionary russianDict;

        public LanguageDetector()
        {
            englishDict = new Dictionary("en");
            russianDict = new Dictionary("ru");
        }

        public string Decision(string word, string lang)
        {   
           return lang;
        }
    }
}
