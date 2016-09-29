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

        public static string GetMALUrl(string query)
        {
            using (WebClient wc = new WebClient())
            {
                string searchPage = wc.DownloadString(@"http://myanimelist.net/anime.php?q=" + query);

                bool linkSectionFound = false;
                int offset = 0;
                string l = null;
                while (!linkSectionFound)
                {
                    int aIndex = searchPage.IndexOf("<a", offset);
                    if (aIndex < 0)
                    {
                        return null;
                    }
                    try
                    {
                        l = searchPage.Substring(aIndex, searchPage.IndexOf(@"</a", aIndex) - aIndex);

                        if (l.Contains("strong"))
                        {
                            linkSectionFound = true;
                        }
                        else
                        {
                            offset = aIndex + 2; //2 = "<a" length
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }
                int hIndex = l.IndexOf("href=\"") + 6;
                return l.Substring(hIndex, l.IndexOf("\"", hIndex) - hIndex);
            }
        }
    }
}
