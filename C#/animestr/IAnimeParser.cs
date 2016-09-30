using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public interface IAnimeParser
    {
        List<AnimeEntry> GetRecommendations();
        void GetEpisodeData();
    }
}
