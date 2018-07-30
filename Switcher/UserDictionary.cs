using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AutoMova.Switcher
{
    public class UserDictionary
    {
        private string[] defaultDictionary;
        private string[] userDictionary;
        private string userDictPath;

        public UserDictionary(string lang)
        {
            var lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\resources\\proto\\" + lang + "\\dictionary");
            defaultDictionary = lines.Where(line => line != "").ToArray();
            userDictPath = string.Format(@"{0}\{1}.txt", Common.UserDictsFolder, lang);
            var userLines = ReadUserDictionary();
            userDictionary = userLines.Where(line => line != "").ToArray();            
        }

        public bool Contains(string word)
        {
            foreach (var regex in defaultDictionary)
            {
                if (Regex.IsMatch(word, regex))
                {
                    Debug.WriteLine($"Word {word} matched to regex {regex}");
                    return true;
                }
            }
            foreach (var dictWord in userDictionary)
            {
                var regex = WordToRegex(dictWord);
                if (Regex.IsMatch(word, regex))
                {
                    Debug.WriteLine($"Word {word} matched to user regex {regex}");
                    return true;
                }
            }
            return false;
        }

        private string WordToRegex(string word)
        {
            return string.Format(@"^{0}$", word);
        }

        private string[] ReadUserDictionary()
        {
            string[] dict = new string[0];            
            if (Common.CheckFile(userDictPath))
            {
                dict = File.ReadAllLines(userDictPath);
            }
            return dict;            
        }

        public bool AddOrRemove(string word)
        {
            if (ExistInUserDict(word))
            {
                Remove(word);
                return false;
            }
            Add(word);
            return true;            
        }

        private bool ExistInUserDict(string word)
        {
            return Array.Exists(userDictionary, el => el == word);
        }

        private void Add(string word)
        {
            File.AppendAllLines(userDictPath, new[] {word});
            userDictionary = userDictionary.Concat(new[] {word}).ToArray();                        
            return;
        }
        
        public void Remove(string word)
        {
            userDictionary = userDictionary.Where(el => el != word).ToArray();
            File.WriteAllLines(userDictPath, userDictionary);
            return;
        }


    }
}
