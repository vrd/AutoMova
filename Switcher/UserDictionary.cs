using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AutoMova.Switcher
{
    public class UserDictionary
    {
        private string[] dictionary; 

        public UserDictionary(string lang)
        {
            dictionary = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\proto\\" + lang + "\\dictionary");
        }

        public bool Contains(string word)
        {
            foreach (var regex in dictionary)
            {
                if (Regex.IsMatch(word, regex))
                {
                    Debug.WriteLine($"Word {word} matched to regex {regex}");
                    return true;
                }
            }

            return false;
        }

        public bool Add(string word)
        {
            return true;
        }

        public bool Remove(string word)
        {
            return true;
        }

    }
}
