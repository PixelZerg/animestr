using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace animestr.Sources
{
    public class AnimeDao : IAnimeSource
    {
        public AnimeData GetData(AnimeEntry entry)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, string> GetMRLs(string epPageUrl)
        {
            throw new NotImplementedException();
        }

        public List<AnimeEntry> GetRecommendations()
        {
            string page = "";
            using (WebClient wc = new WebClient())
            {
                page = wc.DownloadString("http://animedao.me/popular-anime");
            }
            foreach (string section in Parsing.GetSections(page, "<div class=\"well\">", "</div>", "\"row\""))
            {
                Console.WriteLine(section);
                Utils.PrintBreak('-');
            }
            return null;
        }

        public List<AnimeEntry> GetSearchResult(string query)
        {
            throw new NotImplementedException();
        }
    }
}
