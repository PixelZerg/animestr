using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class AnimeInfo
    {
        public string title = null;
        public string description = null;
        public string alts = null;
        public List<string> genres = new List<string>();

        //MAL
        public string score = null;
        public string rank = null;
        public string popularity = null;

        public bool usedMAL { get; private set; }

        public AnimeInfo() { }
        public AnimeInfo(string title)
        {
            this.title = title;
            this.LoadFromMAL();
        }

        public void LoadFromMAL()
        {
            if (title != null)
            {

            }
        }

        public static string GetMALPage(string title)
        {
            using (WebClient wc = new WebClient())
            {
                string searchPage = wc.DownloadString(@"https://myanimelist.net/anime.php?q=" + title);

                string[] split = searchPage.Split(new string[] { "<a" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in split)
                {
                    if (s.Contains("<strong>"))
                    {
                        Console.WriteLine(s);
                        Utils.PrintBreak('-');
                    }
                }
            }

            return null;
        }
    }
}
