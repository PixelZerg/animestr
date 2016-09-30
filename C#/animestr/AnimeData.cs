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
        private AnimeInfo _malInfo = null;
        public AnimeInfo malInfo
        {
            get
            {
                if (_malInfo != null)
                {
                    return _malInfo;
                }
                else
                {
                    if (entry != null)
                    {
                        if (entry.name != null)
                        {
                            _malInfo = new AnimeInfo(entry.name);
                            return malInfo;
                        }
                    }
                }
                return null;
            }
            set { _malInfo = value; }
        }
        public List<EpisodeData> episodes = null; //do not instantiate on initialise - differentiate between no episodes and not parsed
    }
}
