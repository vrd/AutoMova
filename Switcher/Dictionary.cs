using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotSwitcher.Switcher
{
    public class Dictionary
    {
        private string[] wrongTwoSymbols;
        private string[] wrongThreeSymbols;
        //private List<string> correctWords;

        public Dictionary(string lang)
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
                if (IllegalCombinationFound(word.Substring(word.Length - 2), wrongTwoSymbols))
                {
                    return false;
                }
                if (IllegalCombinationFound(word.Substring(word.Length - 3), wrongThreeSymbols))
                {
                    return false;
                }
                return true;
            }

            return true;
        }

        private string[] LoadDictionaryFromFile(string lang, string dictType)
        {
            string[] lines = System.IO.File.ReadAllLines("Langs\\"+lang+"\\"+dictType);
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
