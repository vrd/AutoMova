using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NHunspell;

namespace AutoMova.Switcher
{
    public class LayoutDetector
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

        public string DetectLayout(Dictionary<string, string> lastWord, string currentLang, bool firstWord)
        {
            if (!validLangs.Contains(currentLang))
            {
                Debug.WriteLine($"Unsupported lang: {currentLang.ToUpper()}");
                return currentLang;
            }

            string detectedLang;

            if (firstWord)
            {
                detectedLang = CheckAllUser(lastWord, currentLang);
                if (detectedLang != "none") return detectedLang;
                detectedLang = CheckAllHunspell(lastWord, currentLang);
                if (detectedLang != "none") return detectedLang; 
            }
            else
            {                            
                if (WordBelongsToLang(lastWord[currentLang].Trim(), currentLang)) return currentLang;
                foreach (var lang in validLangs)
                {
                    if (lang == currentLang) continue;
                    if (WordBelongsToLang(lastWord[lang].Trim(), lang)) return lang;                    
                }                
            }

            detectedLang = CheckAllProto(lastWord, currentLang);
            if (detectedLang != "none") return detectedLang;
            return currentLang;
        }
    
        public bool AddOrRemove(string word, string lang)
        {
            if (!validLangs.Contains(lang))
            {
                return false;
            }
            return (userDictionaries[lang].AddOrRemove(word));
        }

        

        private string CheckAllUser(Dictionary<string, string> word, string currentLang)
        {
            if (WordBelongsToDict(word[currentLang].Trim(), userDictionaries[currentLang]))
            {
                Debug.WriteLine($"Word found in current user {currentLang.ToUpper()}");
                return currentLang;
            }
            Debug.WriteLine($"Word not found in current user {currentLang.ToUpper()}");
            foreach (var lang in validLangs)
            {
                if (lang == currentLang) continue;
                if (WordBelongsToDict(word[lang].Trim(), userDictionaries[lang]))
                {
                    Debug.WriteLine($"Word found in user ({lang.ToUpper()})");
                    return lang;
                }
                Debug.WriteLine($"Word not found in user ({lang.ToUpper()})");
            }
            return "none";
        }

        private string CheckAllHunspell(Dictionary<string, string> word, string currentLang)
        {
            if (WordBelongsToDict(word[currentLang].Trim(), hunspellDictionaries[currentLang]))
            {
                Debug.WriteLine($"Word found in current Hunspell {currentLang.ToUpper()}");
                return currentLang;
            }
            Debug.WriteLine($"Word not found in current Hunspell {currentLang.ToUpper()}");
            foreach (var lang in validLangs)
            {
                if (lang == currentLang) continue;                
                if (WordBelongsToDict(word[lang].Trim(), hunspellDictionaries[lang]))
                {
                    Debug.WriteLine($"Word found in Hunspell ({lang.ToUpper()})");
                    return lang;
                }
                Debug.WriteLine($"Word not found in Hunspell ({lang.ToUpper()})");
            }
            return "none";
        }

        private string CheckAllProto(Dictionary<string, string> word, string currentLang)
        {
            if (WordBelongsToDict(word[currentLang].Trim(), protoDictionaries[currentLang]))
            {
                Debug.WriteLine($"Word found in current proto {currentLang.ToUpper()}");
                return currentLang;
            }
            Debug.WriteLine($"Word not found in current proto {currentLang.ToUpper()}");
            foreach (var lang in validLangs)
            {
                if (lang == currentLang) continue;
                if (WordBelongsToDict(word[lang].Trim(), protoDictionaries[lang]))
                {
                    Debug.WriteLine($"Word found in proto ({lang.ToUpper()})");
                    return lang;
                }
                Debug.WriteLine($"Word not found in proto ({lang.ToUpper()})");
            }
            return "none";
        }

        private bool WordBelongsToLang(string word, string lang)
        {
            if (WordBelongsToDict(word, userDictionaries[lang]))
            {
                Debug.WriteLine($"Word found in user ({lang.ToUpper()})");
                return true;
            }
            Debug.WriteLine($"Word not found in user ({lang.ToUpper()})");
            if (WordBelongsToDict(word, hunspellDictionaries[lang]))
            {
                Debug.WriteLine($"Word found in hunspell ({lang.ToUpper()})");
                return true;
            }
            Debug.WriteLine($"Word not found in hunspell ({lang.ToUpper()})");
            return false;
        }

        private bool WordBelongsToDict(string word, UserDictionary dict)
        {
            return dict.Contains(word);
        }

        private bool WordBelongsToDict(string word, Hunspell dict)
        {
            return dict.Spell(word);
        }

        private bool WordBelongsToDict(string word, ProtoDictionary dict)
        {
            return dict.Contains(word.ToLower());
        }        

        private string ToLangCountryCode(string lang)
        {
            return lang.Replace("-", "_");
        }
    }
}
