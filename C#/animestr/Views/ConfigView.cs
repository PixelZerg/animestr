using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr.Views
{
    public class ConfigView
    {
        public ConsoleList clist = null;

        public ConfigView()
        {
            Config.Load();
            clist = new ConsoleList("Config", "Select items to change their values, or go [b]ack");
            clist.items.Add(Utils.PadString("Display non-latin chars ", Console.WindowWidth/2) + " : " + Config.printNonLatinCharacters.ToString());
        }

        public void Show()
        {
            ShowConfig();
        }

        public void ShowConfig()
        {
            Utils.ClearConsole();
            clist.PrintList();
        }
    }
}
