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
        private Dictionary<string, UserDictionary> userDictionaries = new Dictionary<string, UserDictionary>();
        private Dictionary<string, Hunspell> hunspellDictionaries = new Dictionary<string, Hunspell>();
        private Dictionary<string, ProtoDictionary> protoDictionaries = new Dictionary<string, ProtoDictionary>();
        private List<string> validLangs = new List<string>();
        
        public LayoutDetector(string[] langs)
        {
            Debug.WriteLine($"Current path is {AppDomain.CurrentDomain.BaseDirectory}");
            foreach (var langCountry in langs)
            {
                var lang = langCountry.Substring(0, 2);
                try
                {
                    //Load user dictionaries
                    userDictionaries.Add(langCountry, new UserDictionary(lang));

                    //Load Hunspell dictionaries
                    var hunspellPath = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\hunspell\\" + lang + "\\" + ToLangCountryCode(langCountry);
                    var affFile = hunspellPath + ".aff";
                    var dicFile = hunspellPath + ".dic";
                    hunspellDictionaries.Add(langCountry, new Hunspell(affFile, dicFile));

                    //Load proto-dictionaries
                    protoDictionaries.Add(langCountry, new ProtoDictionary(lang));
                }
                catch (Exception e)
                {

                    Debug.WriteLine("!!! Error opening dictionary !!!");
                    continue;
                }

                validLangs.Add(langCountry);                
            }
        }

        public string Decision(Dictionary<string, string> lastWord, string currentLang)
        {
            if (!validLangs.Contains(currentLang))
            {
                return currentLang;
            }

            //check user dictionaries 
            if (userDictionaries[currentLang].Contains(lastWord[currentLang]))
            {
                Debug.WriteLine($"Word found in current user {currentLang.ToUpper()}");
                return currentLang;
            }
            foreach (var lang in validLangs)
            {
                if (lang == currentLang)
                {
                    continue;
                }
                else if (userDictionaries[lang].Contains(lastWord[lang]))
                {
                    Debug.WriteLine($"Word found in user ({lang.ToUpper()})");
                    return lang;
                }
            }

            //check Hunspell
            if (hunspellDictionaries[currentLang].Spell(lastWord[currentLang]))
            {
                Debug.WriteLine($"Word found in current Hunspell {currentLang.ToUpper()}");
                return currentLang;
            }
            foreach (var lang in validLangs)
            {
                if (lang == currentLang)
                {
                    continue;
                }
                else if (hunspellDictionaries[lang].Spell(lastWord[lang]))
                {
                    Debug.WriteLine($"Word found in Hunspell ({lang.ToUpper()})");
                    return lang;
                }
            }

            //check proto dictionaries
            if (protoDictionaries[currentLang].Contains(lastWord[currentLang]))
            {
                Debug.WriteLine($"Word found in current proto {currentLang.ToUpper()}");
                return currentLang;
            }
            foreach (var lang in validLangs)
            {
                if (lang == currentLang)
                {
                    continue;
                }
                else if (protoDictionaries[lang].Contains(lastWord[lang]))
                {
                    Debug.WriteLine($"Word found in proto ({lang.ToUpper()})");
                    return lang;
                }
            }          

            Debug.WriteLine($"Word not found anywhere");
            return currentLang;
        }
        
        private bool WordBelongsToLang(string word, string lang)
        {
            if (userDictionaries[lang].Contains(word))
            {
                Debug.WriteLine($"Word found in user {lang.ToUpper()}");
                return true;
            }
            if (hunspellDictionaries[lang].Spell(word))
            {
                Debug.WriteLine($"Word found in Hunspell {lang.ToUpper()}");
                return true;
            }
            if (protoDictionaries[lang].Contains(word))
            {
                Debug.WriteLine($"Word found in proto {lang.ToUpper()}");
                return true;
            }
            return false;
        }

        private string ToLangCountryCode(string lang)
        {
            return lang.Replace("-", "_");
        }
    }
}
