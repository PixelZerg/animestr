using System;
using System.Net;

namespace animestr
{
    /* TODO:
			 * Read first character, establish what is being inputed from that character and ask for the rest of the input (like "MOOO:" above)
			 * Make the searching system
			 * Make a source selection system with indicators to show if the source can be reached, is working, etc...
			 * Make a search result caching system
			 * Show a recommended anime list on startup
			 * Make a cool ascii art splash screen
			*/

    class MainClass
	{
		public static void Main (string[] args)
		{
            Console.OutputEncoding = System.Text.Encoding.UTF8;


            while (true)
            {
                Console.WriteLine("Enter anime title:");
                MALParser mal = new MALParser(AnimeInfo.GetMALPage(Console.ReadLine()));
                Console.WriteLine("Title: " + mal.GetTitle());
                Console.WriteLine("Alt Titles: " + string.Join(", ", mal.GetAlts()));
                Console.WriteLine("Genres: " + string.Join(", ", mal.GetGenres()));
                Console.WriteLine("Score: " + mal.GetScore());
                Console.WriteLine("Rank: " + mal.GetRank());
                Console.WriteLine("Popularity: " + mal.GetPopularity());
                Console.WriteLine("Description: " + mal.GetDescription());
            }

        }
    }
}
