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
        public string title = null;
        public List<string> mrls = new List<string>();
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

        public void LoadFromMAL(string MALurl)
        {
            //leave out check for episodeNo == -1. It will be caught.

            using (WebClient wc = new WebClient())
            {
                string page = wc.DownloadString(MALurl + "/episode/"+Uri.EscapeDataString(episodeNo.ToString()));
                string section = MALParser.GetSection(page, "<div ", "</div>", "Synopsis</h2>");
                string moo = MALParser.GetBetween(section, "</h2>", consts.noString);
                Console.WriteLine(moo);
            }

        }
    }
}
