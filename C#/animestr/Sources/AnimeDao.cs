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
            AnimeData ret = new AnimeData();
            ret.entry = entry;
            ret.info = new AnimeInfo();
            ret.info.title = ret.entry.title;

            //TODO: do more info parsing

            string page = "";
            using (WebClient wc = new WebClient())
            {
                page = wc.DownloadString(entry.url);
            }
            ret.info.genres = new List<string>();
            //get genres
            foreach (string sec in Parsing.GetSections(page,"<a ","</a>","animeinfo_label","genre"))
            {
                try
                {
                    ret.info.genres.Add(Parsing.GetBetween(Parsing.GetSection(sec, "<span ", "</span>"), "class=\"label animedao-color\">", "</span>").Trim());
                }
                catch
                {
                }

            }

            //get desc

            string[] infoSecSpl = Parsing.GetSection(page, "col-lg-9\">", "</div>", "Alternative", "Description").Split(new string[]{ "<br" }, StringSplitOptions.RemoveEmptyEntries);
            //var infoSecSpl = Parsing.GetBetweens(Parsing.GetSection(page, "col-lg-9\">", "</div>", "Alternative", "Description"), "<b", "<br");
            for (int i = 0; i < infoSecSpl.Length; i++)
            {
                if (infoSecSpl[i].Contains("Description"))
                {
                    try
                    {
                        ret.info.description = Parsing.Format(Parsing.GetBetween(infoSecSpl[i + 1], ">", "<")).Trim();
                    }
                    catch
                    {
                    }
                }
            }

            List<EpisodeData> eps = new List<EpisodeData>();

            int no = 0;
            foreach (var dom in new CQ(page).Select(".animeinfo-content"))
            {
                try
                {
                    EpisodeData ep = new EpisodeData();
                    ep.episodeNo = no;
                    ep.title = Parsing.Format(Parsing.GetBetween(dom.InnerHTML, "<b>", "</b>").Trim());
                    ep.pageLink = Parsing.AbsUri(Consts.AD_BASE, dom.GetAttribute("href"));
                    eps.Add(ep);
                }
                catch
                {
                }
                no++;
            }

            ret.episodes = eps;

            return ret;
        }

        public Dictionary<string, string> GetMRLs(string epPageUrl)
        {
            //TODO: review

            Dictionary<string, string> ret = new Dictionary<string, string>();

            string page = "";
            using (WebClient wc = new WebClient())
            {
                page = wc.DownloadString(epPageUrl);
            }

            string script = Parsing.GetSection(page, "<script>", "</script>", "dfea");
            //Console.WriteLine(script);

            foreach (string s in Parsing.GetBetweens(script, "var", ".dfea"))
            {
                string videoCode = Utils.ROT13(Parsing.GetBetween(s, "\"", "\""));
                string name = s.Substring(0, s.IndexOf("\"") - 2).Trim();
                ret.Add(name, Consts.AD_BASE + "/redirect/" + videoCode);
            }

            return ret;
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
                    entry.title = Parsing.Format(Parsing.GetBetween(Parsing.GetBetween(dom.InnerHTML, "<h3>", "</h3>"), ">", "<").Trim());

                    ret.Add(entry);
                }
                catch
                {
                }
            }

            return ret;
        }

        public List<AnimeEntry> GetSearchResults(string query)
        {
            List<AnimeEntry> ret = new List<AnimeEntry>();

            string page = "";
            using (WebClient wc = new WebClient())
            {
                page = wc.DownloadString(Consts.AD_SEARCH + "/" + Uri.EscapeDataString(query));
            }

            foreach (var dom in new CQ(page).Select(".animelist_poster"))
            {
                try
                {
                    AnimeEntry entry = new AnimeEntry();

                    entry.url = Parsing.AbsUri(Consts.AD_BASE, Parsing.GetBetween(dom.InnerHTML, "href=\"", "\""));
                    entry.pictureUrl = Parsing.AbsUri(Consts.AD_BASE, Parsing.GetBetween(dom.InnerHTML, "data-original=\"", "\""));
                    entry.title = Parsing.Format(Parsing.GetBetween(Parsing.GetBetween(dom.InnerHTML, "<center>", "</center>"), ">", "<").Trim());

                    ret.Add(entry);
                }
                catch
                {
                }
            }

            return ret;
        }
    }
}
