using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CsQuery;

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
            List<AnimeEntry> ret = new List<AnimeEntry>();

            string page = "";
            using (WebClient wc = new WebClient())
            {
                page = wc.DownloadString(Consts.AD_POPULAR);
            }

            foreach (var dom in new CQ(page).Select(".well .row"))
            {
                try
                {
                    AnimeEntry entry = new AnimeEntry();

                    entry.url = Parsing.AbsUri(Consts.AD_BASE, Parsing.GetBetween(dom.InnerHTML, "href=\"", "\""));
                    entry.pictureUrl = Parsing.AbsUri(Consts.AD_BASE, Parsing.GetBetween(dom.InnerHTML, "data-original=\"", "\""));
                    entry.name = Parsing.GetBetween(Parsing.GetBetween(dom.InnerHTML, "<h3>", "</h3>"),">","<").Trim();

                    ret.Add(entry);
                }
                catch { }
            }

            return ret;
        }

        public List<AnimeEntry> GetSearchResults(string query)
        {
            throw new NotImplementedException();
        }
    }
}
