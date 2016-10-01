using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class Display
    {
        public IAnimeSource source = null;

        //set to null when not in use so invalid commands can be handled properly
        private ConsoleList clist = null;
        private List<AnimeEntry> curEntryList = null;

        public Display()
        {
            source = new Sources.AnimeDao();
        }

        public void Show()
        {
            ShowRecomendations();
        }

        public void ReadCommand()
        {
            Console.ForegroundColor = Console.BackgroundColor;
            ConsoleKeyInfo k = Console.ReadKey();

            if (Char.IsNumber(k.KeyChar) && clist != null) //selecting something in the clist
            {
                Utils.ClearConsole();

                clist.PrintList(1);

                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Select:");
                Console.ResetColor();
                Console.Write(" " + k.KeyChar);
                try
                {
                    int selNo = Int32.Parse(k.KeyChar + Console.ReadLine());
                }
                catch
                {
                    InvalidCommand("Invalid index!");
                }
            }
            else if (k.Key == ConsoleKey.H || k.KeyChar == '?')
            {
                Utils.ClearConsole();
                ShowHelp();
                ReadCommand();
            }
            else
            {
                InvalidCommand();
            }
        }

        private void InvalidCommand(string msg = "Invalid command!")
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg+" Do ? or h for help");
            Console.ResetColor();
            ReadCommand();
        }

        public void ShowHelp()
        {
            Console.WriteLine("HELP:");
            Console.WriteLine("Search for anime by starting your command with '/' or '.'");
            Console.WriteLine("If you are looking at a list, you can SELECT an item by entering the item's index in the list.");
        }

        private void ShowRecomendations()
        {
            List<AnimeEntry> recommendations = new List<AnimeEntry>();

            new System.Threading.Thread(() =>
            {
                DisplaySplash("Loading recommendations...");
            }).Start();
            recommendations = source.GetRecommendations();
            splashDone = true;

            clist = new ConsoleList("Recommendations");
            foreach (AnimeEntry recommendation in recommendations)
            {
                clist.items.Add(recommendation.title);
            }
            curEntryList = recommendations;
            clist.PrintList(1);

            ReadCommand();
        }

        public bool splashDone = false;
        public void DisplaySplash(string text, bool resetColour = false)
        {
            splashDone = false;
            if (Console.WindowHeight > 8 && Console.WindowWidth > 52)
            {
                int no = 0;
                while (!splashDone)
                {
                    string[][] frames = (no % 2 != 0 ? Consts.ASCII_TEXT_FRAMES_REV : Consts.ASCII_TEXT_FRAMES);
                    foreach (string[] frame in frames)
                    {
                        Utils.ClearConsole();

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        int offy = ((Console.WindowHeight - 6) / 2) + 1;

                        int offx = (Console.WindowWidth - 52) / 2;
                        foreach (string row in frame)
                        {
                            for (int i = 0; i < offx; i++)
                            {
                                Console.Write(' ');
                            }
                            Console.WriteLine(row);
                        }
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        string s = "v" + Consts.VERSION;
                        for (int i = 0; i < offx + (39 - s.Length); i++)//39=3/4 of 52/
                        {
                            Console.Write(' ');
                        }
                        Console.WriteLine(s);
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        for (int i = 0; i < (Console.WindowWidth / 2) - (text.Length / 2); i++)
                        {
                            Console.Write(' ');
                        }
                        Console.WriteLine(text);

                        for (int i = 0; i < offy; i++)
                        {
                            Console.WriteLine();
                        }

                        if (resetColour)
                        {
                            Console.ResetColor();
                        }
                        for (int i = 0; i < 200; i++)
                        {
                            if (!splashDone)
                            {
                                System.Threading.Thread.Sleep(1); //did it this way so that the thread will stop within 1 ms of the value being changed
                            }
                            else
                            {
                                if (resetColour)
                                {
                                    Console.ResetColor();
                                }
                                return;
                            }
                        }
                    }
                    no++;
                }
            }
            else
            {
                for (int i = 0; i < (Console.WindowWidth / 2) - (Consts.VERSION.Length + 10) / 2; i++)
                {
                    Console.Write(' ');
                }
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("animestr");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(" v" + Consts.VERSION);
                

                Console.WriteLine();
                for (int i = 0; i < (Console.WindowWidth / 2) - (text.Length / 2); i++)
                {
                    Console.Write(' ');
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(text);

                for (int i = 0; i < (Console.WindowHeight / 2) + 1; i++)
                {
                    Console.WriteLine();
                }
                if (resetColour)
                {
                    Console.ResetColor();
                }
                while (!splashDone) {}
            }

        }

    }
}
