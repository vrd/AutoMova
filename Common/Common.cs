using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AutoMova.Switcher
{
    public static class Common
    {
        // Get the user dictionaries folder
        public static string UserDictsFolder
        {
            get
            {
                string dir = string.Format(@"{0}\{1}\", UserRoamingDataFolder, "user_dicts");
                return CheckDir(dir);
            }
        }

        // Get the current user roaming data folder
        public static string UserRoamingDataFolder
        {
            get
            {
                string folderBase = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string dir = string.Format(@"{0}\{1}\", folderBase, "AutoMova");
                return CheckDir(dir);
            }
        }

        // Check the specified folder, and create if it doesn't exist.
        private static string CheckDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            return dir;
        }

        // Check the specified folder, and create if it doesn't exist.
        public static bool CheckFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);
                return false;
            }
            return true;
        }
    }
}
