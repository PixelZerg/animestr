using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
