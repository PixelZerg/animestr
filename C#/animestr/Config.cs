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
        public static bool displayUnicodeTitles = false;
        public static string playProgram = "vlc";
        public static string playArgs = "%url%";

        private static FileInfo file = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "/.animestr");

        private static string GetDataString()
        {
            string ret = "";
            ret += (displayUnicodeTitles ? 1 : 0) + "!";
            ret += Convert.ToBase64String(Encoding.UTF8.GetBytes(playProgram)) + "!";
            ret += Convert.ToBase64String(Encoding.UTF8.GetBytes(playArgs)) + "!";
            return ret;
        }

        private static void LoadDataString(string s)
        {
            string[] data = s.Split('!');
            displayUnicodeTitles = data[0].StartsWith("1");
            playProgram = Encoding.UTF8.GetString(Convert.FromBase64String(data[1]));
            playArgs = Encoding.UTF8.GetString(Convert.FromBase64String(data[2]));
        }

        public static void Load()
        {
            try
            {
                if (file.Exists)
                {
                    LoadDataString(File.ReadAllText(file.FullName));
                    return;
                }
            }
            catch
            {
            }
            Save(); //save with default values
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
