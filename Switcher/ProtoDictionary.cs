using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoMova.Switcher
{
    public class ProtoDictionary
    {
        private string[] wrongTwoSymbols;
        private string[] wrongThreeSymbols;
        //private List<string> correctWords;

        public ProtoDictionary(string lang)
        {
            wrongTwoSymbols = LoadDictionaryFromFile(lang, "proto");
            wrongThreeSymbols = LoadDictionaryFromFile(lang, "proto3");
            //correctWords = LoadDictionaryFromFile(lang, "dictionary");
        }

        public bool Contains(string word)
        {   
            if (word.Length == 0 || word.Length == 1)
            {
                return true; 
            }

            if (word.Length == 2)
            {
                if (IllegalCombinationFound(word, wrongTwoSymbols))
                {
                    return false;
                }
                return true;
            }

            if (word.Length >= 3)
            {
                for (int i = 0; i < word.Length - 1; i++)
                {
                    if (IllegalCombinationFound(word.Substring(i, 2), wrongTwoSymbols))
                        {
                            return false;
                        }
                }

                for (int i = 0; i < word.Length - 2; i++)
                {
                    if (IllegalCombinationFound(word.Substring(i,3), wrongThreeSymbols))
                        {
                            return false;
                        }
                }
                
                return true;
            }

            return true;
        }

        private string[] LoadDictionaryFromFile(string lang, string dictType)
        {
            string[] lines = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory +"\\proto\\" +lang+"\\"+dictType);
            return lines;
        }

        private bool IllegalCombinationFound(string testComb, string[] dict)
        {
            foreach (var comb in dict)
            {
                if (testComb == comb)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
