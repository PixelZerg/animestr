using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public interface IAnimeSource
    {
        /// <summary>
        /// Parse video URLs, given, the episode page's URL.
        /// </summary>
        Dictionary<string, string> GetMRLs(string epPageUrl);
        /// <summary>
        /// Return an AnimeData with populated info (basic info), if available on the page and episode names 
        /// </summary>
        AnimeData GetData(AnimeEntry entry);

        List<AnimeEntry> GetRecommendations();
        List<AnimeEntry> GetSearchResult(string query);

    }
}
