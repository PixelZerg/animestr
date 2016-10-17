using System;
using System.Linq;

namespace animestr
{
    public class AnimeDataView
    {
        public AnimeData data = null;
        public IAnimeSource source = null;
        public Display display = null;

        public bool[] itemsDisplayed = new bool[]{ false, false, false, false, false };
        //p, o, d, i, b

        public AnimeDataView(Display display, AnimeData data)
        {
            this.display = display;
            this.source = display.source;
            this.data = data;
        }

        public void Show()
        {
            ShowData();
            this.ReadCommand();
        }

        public void ReadCommand()
        {
            Console.ForegroundColor = Console.BackgroundColor;
            DoCommand(Console.ReadKey());
        }

        public void DoCommand(ConsoleKeyInfo k)
        {
            if (k.Key == ConsoleKey.I && itemsDisplayed[3])
            {
                this.display.ShowSplash("Loading information...");   
                data.info.LoadFromMAL();
                this.display.splashDone = true;
                
                this.ShowData();
                ReadCommand();
            }
            else if (k.Key == ConsoleKey.D && itemsDisplayed[2])
            {
                //show description
                this.ShowMessage(this.data.entry.title + " - Description", this.data.info.description);
                this.ShowData();
                ReadCommand();
            }
            else if (k.Key == ConsoleKey.O && itemsDisplayed[1])
            {
                this.display.ShowSplash("Opening page...");
                string file = "";
                try
                {
                    if (data.info.MALPage != null)
                    {
                        file = System.IO.Path.GetTempPath() + Utils.MakeValidFileName(data.info.MALUrl);
                        using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Create))
                        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fs))
                        {
                            sw.WriteLine(data.info.MALPage);
                        } 
                    }
                    else if (data.info.MALUrl != null)
                    {
                        file = data.info.MALUrl;
                    }
                }
                catch
                {
                }

                if (file != "")
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo = new System.Diagnostics.ProcessStartInfo(file);
                    p.Start();
                    p.WaitForInputIdle();
                    this.display.splashDone = true;
                    this.ShowData();
                    this.ReadCommand();
                }
                else
                {
                    this.display.splashDone = true;
                    InvalidCommand("Something went wrong!");
                }
            }
            else if (k.Key == ConsoleKey.P && itemsDisplayed[0])
            {
                InvalidCommand("Comming soon!");
            }
            else if (k.Key == ConsoleKey.R)
            {
                Utils.ClearConsole();
                this.ShowData();
                ReadCommand();
            }
            else if (k.Key == ConsoleKey.Enter && itemsDisplayed[4])
            {
                ShowEpisodes();
                //ReadCommand();
                Utils.ClearConsole();
                this.ShowData();
                this.ReadCommand();
            }
            else if (k.Key == ConsoleKey.B || k.Key == ConsoleKey.Backspace)
            {
                //exit the methodcall
            }
            else
            {
                InvalidCommand();
            }
        }

        public void ShowEpisodes()
        {
//            InvalidCommand("Comming soon!");
            // this.data.episodes[this.data.episodes.Count - 1].mrls = this.source.GetMRLs(this.data.episodes[this.data.episodes.Count - 1].pageLink.AbsoluteUri);
            //Console.WriteLine(string.Join(Environment.NewLine, this.data.episodes));
            new EpisodesView(this.display, this.data).Show();
        }

        public void ShowMessage(string title, string message)
        {
            Utils.ClearConsole();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Utils.PrintSeperator(title, '-', '|');
            int lines = 1;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
            lines += (int)Math.Ceiling(message.Length / (double)(Console.WindowWidth - 1));
            lines += Utils.CountInstances(message, "\n");
            Console.WriteLine();
            lines += 3;
            if (lines < Console.WindowHeight)
            {
                for (int i = 0; i < Console.WindowHeight - lines; i++)
                {
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Press any key to go back");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.ReadKey();
        }

        private void InvalidCommand(string msg = "Invalid command!")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg + " Try again.");
            Console.ResetColor();
            ReadCommand();
        }

        public void ShowData()
        {                  
            //data.info.LoadFromMAL();
            display.splashDone = true;
            //Console.ReadKey();
            Utils.ClearConsole();
            int lines = 0;
            if (data.info.title != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Utils.PrintSeperator(data.entry.title, '-', '|');
                Console.ResetColor();
                lines++;
            }
            if (data.info.score != null && data.info.rank != null && data.info.popularity != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Score: ");
                Console.Write(data.info.score);
                for (int i = 0; i < ((Console.WindowWidth - 2) / 3) - 7 - data.info.score.Length; i++)
                {
                    Console.Write(' ');
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("| Rank: ");
                Console.Write(data.info.rank);
                for (int i = 0; i < ((Console.WindowWidth - 2) / 3) - 8 - data.info.rank.Length; i++)
                {
                    Console.Write(' ');
                }
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("| Popularity: ");
                Console.Write(data.info.popularity);
                for (int i = 0; i < ((Console.WindowWidth - 2) / 3) - 14 - data.info.rank.Length; i++)
                {
                    Console.Write(' ');
                }
                Console.Write(Environment.NewLine);
                lines++;
            }
            if (data.entry.url != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("Url: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(data.entry.url);
                Console.ResetColor();
                lines++;
            }
            if (data.entry.pictureUrl != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("View picture with 'p'");
                Console.ResetColor();
                itemsDisplayed[0] = true;
                lines++;
            }
            if (data.info.MALUrl != null || data.info.MALPage != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Open myanimelist page with in browser with 'o'");
                Console.ResetColor();
                itemsDisplayed[1] = true;
                lines++;
            }
            if (data.info.alts != null)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                string s = "Alternate titles: " + string.Join(", ", data.info.alts.Where(x => !Utils.ContainsUnicode(x) || Config.displayUnicodeTitles));
                Console.WriteLine(s);
                int slength = new System.Globalization.StringInfo(s).LengthInTextElements;
                lines += (int)Math.Ceiling(slength / (double)(Console.WindowWidth - 1));
                //TODO:NB: Still cannot fix long JP characters like ー counting issues
                //TODO: make settings to turn off JP characters.
            }
            if (data.info.genres != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                string s = "Genres: " + string.Join(", ", data.info.genres);
                Console.WriteLine(s);
                lines += (int)Math.Ceiling(s.Length / (double)(Console.WindowWidth - 1));
            }
            if (data.info.description != null)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Description: ");
                string extract = (data.info.description.Split('\n')[0].Length > Console.WindowWidth - 4) ? data.info.description.Split('\n')[0].Substring(0, Console.WindowWidth - 4) : data.info.description.Split('\n')[0];
                extract = (extract.EndsWith(".") ? extract + ".." : extract + "...");
                Console.WriteLine(extract);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("View full description with 'd'");
                lines += 3;
                itemsDisplayed[2] = true;
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Load information from MAL with 'i'");
            lines += 1;
            itemsDisplayed[3] = true;


            Console.ResetColor();
            lines += 2;
            for (int i = 0; i < Console.WindowHeight - lines; i++)
            {
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Go back with 'b' (or BACKSPACE) or view episodes with (ENTER)");
            itemsDisplayed[4] = true;

        }
    }
}

