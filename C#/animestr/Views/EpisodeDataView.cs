using System;
using System.Linq;

namespace animestr
{
    public class EpisodeDataView
    {
        public IAnimeSource source = null;
        public Display display = null;
        public EpisodeData data = null;

        public ConsoleList clist = null;

        public EpisodeDataView(Display display, EpisodeData data)
        {
            this.display = display;
            this.source = display.source;
            this.data = data;

            display.ShowSplash("Loading episode's sources...");

            this.data.mrls = this.source.GetMRLs(this.data.pageLink.AbsoluteUri);

            clist = new ConsoleList("Sources", "Select which source to use");

            foreach (var pair in data.mrls)
            {
                clist.items.Add(pair.Key);
            }
            display.splashDone = true;
        }

        public void Show()
        {
            ShowData();
            ReadCommand();
        }

        public void ShowData()
        {
            clist.PrintList();
        }

        public void ReadCommand()
        {
            Console.ForegroundColor = Console.BackgroundColor;
            DoCommand(Console.ReadKey());
        }

        public void DoCommand(ConsoleKeyInfo k)
        {
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
                        this.SelectSource(Int32.Parse(Console.ReadLine()) - 1);
                        //Utils.ClearConsole();
                        //clist.PrintList();
                        //this.ReadCommand();
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
                        if (clist.curSel > clist.items.Count - 1)
                        {
                            clist.curSel = clist.items.Count - 1;
                        }  
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
                    this.SelectSource(clist.curSel);
                    //Utils.ClearConsole();
                    //clist.PrintList();
                    //this.ReadCommand();
                    return;   
                }
                else if (k.KeyChar == '>' || k.Key == ConsoleKey.RightArrow)
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
                else if (k.Key == ConsoleKey.P)
                {
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
                        if (no <= clist.pageCount && no > 0)
                        {
                            clist.curPageNo = no;
                            clist.PrintList();
                            ReadCommand();
                        }
                        else
                        {
                            throw new IndexOutOfRangeException();
                        }
                    }
                    catch
                    {
                        InvalidCommand("Please enter an integer within the range 1-" + clist.pageCount + "!");
                    }
                    return;
                }
            }

            if (k.Key == ConsoleKey.B || k.Key == ConsoleKey.Backspace)
            {
                return;
            }
            else
            {
                InvalidCommand();
            }
        }

        private void SelectSource(int index)
        {
            if (index >= 0 && index < this.data.mrls.Count)
            {
                string url = this.data.mrls.Values.ElementAt(index);
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(Config.playProgram, Config.playArgs.Replace("%url%", url));
                p.WaitForExit();
            }
            else
            {
                throw new Exception();
            }
        }

        private void InvalidCommand(string msg = "Invalid command!")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg + " Try again.");
            Console.ResetColor();
            ReadCommand();
        }
    }
}

