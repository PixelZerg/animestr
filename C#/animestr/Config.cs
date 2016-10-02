using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class Config
    {
        public static bool printNonLatinCharacters = false;

        private static FileInfo file = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.animestr");

        private static string GetDataString()
        {
            string ret = "";
            ret += (printNonLatinCharacters ? 1 : 0) + "!";
            return ret;
        }

        private static void LoadDataString(string s)
        {
            string[] data = s.Split('!');
            printNonLatinCharacters = data[0].StartsWith("1");
        }

        public static void Load()
        {
            if (file.Exists)
            {
                LoadDataString(File.ReadAllText(file.FullName));
            }
            else
            {
                Save(); //save with default values
            }
        }

        public static void Save()
        {
            using (FileStream fs = new FileStream(file.FullName, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(GetDataString());
            }
        }

    }
}
