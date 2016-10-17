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
//            clist.items.Add(Utils.PadString("Show titles with unicode chars (can mess up display) ", (Console.WindowWidth / 4) * 3) + " : " + Config.displayUnicodeTitles.ToString());
//            clist.items.Add(Utils.PadString("Program to play MRL ", (Console.WindowWidth / 4) * 2) + " : " + Config.playProgram.ToString());
//            clist.items.Add(Utils.PadString("Arguments - %url% will be replaced w/MRL", (Console.WindowWidth / 4) *2) + " : " + Config.playArgs.ToString());
            clist.items.Add("Show titles with unicode chars (can mess up display) : " + Config.displayUnicodeTitles.ToString());
            clist.items.Add("Program to play MRL : " + Config.playProgram.ToString());
            clist.items.Add("Arguments (%url% will be replaced w/MRL) : " + Config.playArgs.ToString());
        }

        public void Show()
        {
            ShowConfig();
            Console.ReadKey();
        }

        public void ShowConfig()
        {
            Utils.ClearConsole();
            clist.PrintList();
        }
    }
}
