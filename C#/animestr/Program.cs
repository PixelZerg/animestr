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
             * Fix URL overflow on AnimeDataView (not important)
			*/

    class MainClass
	{
		public static void Main (string[] args)
		{
            Config.Load();
            new Display().Show();
            //TODO!: split the Dislay class into multiple ...View classes. E.g MainView will have the searching, etc. AnimeView show AnimeData, EpisodeView will show episodes, etc... 
            //TODO!: Update selection to s{no} and arrow keys
            //TODO!: do settingsView
        }
    }
}
