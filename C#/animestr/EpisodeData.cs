using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class EpisodeData
    {
        public string title = "unkown";
        public Uri pageLink = null;
        public Dictionary<string, string> mrls = new Dictionary<string, string>(); //e.g: "Source 1 - 360p", "www.video.mp4"
        public string synopsis = null;
        public int episodeNo = -1;

        //TODO: duration, airing date

        public EpisodeData()
        {
        }

        public EpisodeData(int episodeNo)
        {
            this.episodeNo = episodeNo;
        }

        public EpisodeData LoadFromMAL(string MALurl)
        {
            //leave out check for episodeNo == -1. It will be caught.
            using (WebClient wc = new WebClient())
            {
                string page = wc.DownloadString(MALurl + "/episode/" + Uri.EscapeDataString(episodeNo.ToString()));
                this.synopsis = "";
                try
                {
                    foreach (string s in MALParser.GetBetweens(MALParser.GetBetween(MALParser.GetSection(page, "<div ", "</div>", "Synopsis</h2>"), "</h2", consts.noString), ">", "<"))
                    {
                        synopsis += MALParser.Format(s).Trim() + Environment.NewLine;
                    }
                }
                catch
                {
                    this.synopsis = "Unable to get synopsis.";
                }

                string titleSection = MALParser.GetSection(page, "<h2 ", "</h2>", "fs", "#");
                int lIndex = titleSection.LastIndexOf('>')+1;
                this.title = titleSection.Substring(lIndex, titleSection.Length - lIndex);
            }
            return this;
        }
    }
}
