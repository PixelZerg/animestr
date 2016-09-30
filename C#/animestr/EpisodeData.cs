using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
