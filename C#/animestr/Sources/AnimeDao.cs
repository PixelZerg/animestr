using System;
using System.Collections.Generic;
using System.Linq;
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
            throw new NotImplementedException();
        }

        public List<AnimeEntry> GetSearchResult(string query)
        {
            throw new NotImplementedException();
        }
    }
}
