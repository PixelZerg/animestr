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
            DoCommand(Console.ReadKey());
        }

        public void SelectAnime(int index){
            if (index <= clist.items.Count && index > 0)
            {
                //select an anime
                new System.Threading.Thread(() =>
                    {
                        this.DisplaySplash("Loading anime...");
                    }).Start();
                new AnimeDataView(this, source.GetData(curEntryList[index - 1])).Show();
                this.Refresh();
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public void DoCommand(ConsoleKeyInfo k){
            if (clist != null) //selecting something in the clist
            {
                if (k.Key == ConsoleKey.Spacebar)
                {
                    Utils.ClearConsole();

                    clist.PrintList();

                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Select:");
                    Console.ResetColor();
                    Console.Write(" ");
                    try
                    {
                        this.SelectAnime(Int32.Parse(Console.ReadLine()));
                    }
                    catch
                    {
                        InvalidCommand("Please enter an integer within the range 1-" + clist.items.Count + "!");
                    }
                    return;
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    if (clist.curSel < (clist.curPageNo - 1) * clist.pageSize || clist.curSel >= (clist.curPageNo) * clist.pageSize)
                    {
                        clist.curSel = (clist.curPageNo - 1) * clist.pageSize;   
                    }
                    else
                    {
                        clist.curSel += (clist.curSel + 1 >= 0 && clist.curSel + 1 < clist.items.Count) ? 1 : 0;
                    }
                    if (clist.curSel >= clist.curPageNo * clist.pageSize)
                    {
                        clist.curPageNo++;   
                    }
                    Utils.ClearConsole();
                    clist.PrintList();
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Select:");
                    Console.ResetColor();
                    Console.Write(" " + (clist.curSel + 1));
                    ReadCommand();
                    return;
                }
                else if (k.Key == ConsoleKey.UpArrow)
                {
                    if (clist.curSel < (clist.curPageNo - 1) * clist.pageSize || clist.curSel >= (clist.curPageNo) * clist.pageSize)
                    {
                        clist.curSel = ((clist.curPageNo) * clist.pageSize) - 1;   
                    }
                    else
                    {
                        clist.curSel -= (clist.curSel - 1 >= 0) ? 1 : 0;
                    }
                    if (clist.curSel < (clist.curPageNo - 1) * clist.pageSize)
                    {
                        clist.curPageNo--;   
                    }
                    Utils.ClearConsole();
                    clist.PrintList();
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Select:");
                    Console.ResetColor();
                    Console.Write(" " + (clist.curSel + 1));
                    ReadCommand();
                    return;
                }
                else if (k.Key == ConsoleKey.Enter)
                {
                    this.SelectAnime(clist.curSel + 1);   
                }
                else if ( k.KeyChar == '>' || k.Key == ConsoleKey.RightArrow)
                {
                    Utils.ClearConsole();
                    if (clist.curPageNo + 1 <= clist.pageCount)
                    {
                        clist.curPageNo++;
                    }
                    clist.PrintList(clist.curPageNo);
                    ReadCommand();
                    return;
                }
                else if (k.KeyChar == '<' || k.Key == ConsoleKey.LeftArrow)
                {
                    Utils.ClearConsole();

                    if (clist.curPageNo - 1 > 0)
                    {
                        clist.curPageNo--;
                    }

                    clist.PrintList(clist.curPageNo);
                    ReadCommand();
                    return;
                }
                else if(k.Key == ConsoleKey.P){
                    Utils.ClearConsole();

                    clist.PrintList();

                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Page no:");
                    Console.ResetColor();
                    Console.Write(" ");
                    try
                    {
                        int no = Int32.Parse(Console.ReadLine());
                        if(no <= clist.pageCount&&no>0){
                            clist.curPageNo = no;
                            clist.PrintList();
                            ReadCommand();
                        }else{
                            throw new IndexOutOfRangeException();
                        }
                    }
                    catch
                    {
                        InvalidCommand("Please enter an integer within the range 1-"+clist.pageCount+"!");
                    }
                    return;
                }
            }

            if (k.Key == ConsoleKey.H || k.KeyChar == '?')
            {
                Utils.ClearConsole();
                ShowHelp();
                ReadCommand();
            }
            else if (k.Key == ConsoleKey.S)
            {
                new Views.ConfigView().Show();
            }
            else if (k.Key == ConsoleKey.R)
            {
                Refresh();
                return;
            }
            else
            {
                #if DEBUG
                Console.WriteLine("Entered command: " + k.Key);
                #endif
                InvalidCommand();
            }
        }

        public void Refresh(){
            if (clist != null)
            {
                Utils.ClearConsole();
                clist.PrintList();
                ReadCommand();
            }
            else
            {
                InvalidCommand("Could not refresh!");
            }
        }

        private void InvalidCommand(string msg = "Invalid command! Do [h]elp or [s]ettings.")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg+" Try again.");
            Console.ResetColor();
            ReadCommand();
        }

        public void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("HELP:");
            Console.WriteLine("Access help by 'h' or '?'");
            Console.WriteLine("Search for anime by entering /{query}");
            Console.WriteLine("Use 'r' to refresh the display");
            Console.WriteLine();
            Console.WriteLine("List Interaction:");
            Console.WriteLine("SELECT an item by using the arrow keys and enter");
            Console.WriteLine("OR by entering (SPACE){item index}.");
            Console.WriteLine("Change pages using '>' and '<' or the arrow keys (arrow keys might not work on some terminals)");
            Console.WriteLine("Switch to a specific page by using p{pageNo}");
            Console.ResetColor();
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

            clist = new ConsoleList("Recommendations","Enter /{query} to search, [s]ettings to edit the config or [h]elp for more info");
            foreach (AnimeEntry recommendation in recommendations)
            {
                clist.items.Add(recommendation.title);
            }
            curEntryList = recommendations;
            this.Refresh();

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
