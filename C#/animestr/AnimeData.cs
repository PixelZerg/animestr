using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class AnimeData
    {
        public AnimeEntry entry = null;
        public AnimeInfo info = null;
        public List<EpisodeData> episodes = null; //do not instantiate on initialise - differentiate between no episodes and not parsed
    }
}
