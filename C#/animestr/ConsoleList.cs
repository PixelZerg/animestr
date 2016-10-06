using System;
using System.Collections.Generic;

namespace animestr
{
    public class ConsoleList
    {
        public List<string> items = new List<string>();
        public string title = "list";
        public string desc = "";
        /// <summary>
        /// The current sel. Zero based
        /// </summary>
        public int curSel = 0;

        /// <summary>
        /// The current page no. Non-Zero based
        /// </summary>
        public int curPageNo = 1;

        public int pageCount
        {
            get
            {
                return (items.Count + this.pageSize - 1) / this.pageSize; 
            }
        }

        public int pageSize
        {
            get
            {
                return (Console.WindowHeight - 2 - 3);
            }
        }

        public ConsoleList()
        {
        }

        public ConsoleList(string title, string desc)
        {
            this.title = title;
            this.desc = desc;
        }

        /// <summary>
        /// Prints the list. NB: pageNo is 1-based. It starts with a 1 not a 0!
        /// </summary>
        /// <param name="offtop">Offtop.</param>
        /// <param name="offbottom">Offbottom.</param>
        /// <param name="pageNo">Page no.</param>
        private void PrintList(int offtop, int offbottom, int pageNo)
        {	
            offbottom += 1; //padding
             
            for (int i = pageSize * (pageNo - 1); i < ((pageSize * (pageNo - 1)) + pageSize); i++)
            {
                if (i == curSel)
                {
                    Console.BackgroundColor = ConsoleColor.Cyan;
                }
                if (i % 2 == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (i < items.Count)
                {
                    string s = (i < 9 ? " " : "") + (i < 99 ? " " : "") + (i < 999 ? " " : "") + (i + 1) + " |  " + (items[i].Length >= Console.WindowWidth - 9 - i.ToString().Length ? items[i].Substring(0, Console.WindowWidth - 11 - i.ToString().Length) + "..." : items[i]);
                    Console.Write(s);
                    if (i == curSel)//minor optimisation
                    {
                        for (int j = 0; j < Console.WindowWidth - s.Length; j++)
                        {
                            Console.Write(' ');//padding   
                        }
                    }
                    Console.ResetColor();
                    if (i != curSel || curSel == this.items.Count - 1)//for some reason...?
                    {
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine();//padding
                }
                Console.ResetColor();
            }
        }

        //
        //		/// Prints the list. NB: pageNo is 1-based. It starts with a 1 not a 0!
        //		/// </summary>
        //		public void PrintList(int pageNo)
        //		{
        //            PrintHeader(pageNo);
        //            Console.ResetColor();
        //            Utils.PrintBreak('-');
        //            PrintList(2, 0, pageNo);
        //		}

        //        public void PrintList(int pageNo){
        //            PrintList(this.desc, pageNo);
        //        }
        //made redundant ^^

        /// <summary>
        /// Prints the list. NB: pageNo is 1-based. It starts with a 1 not a 0!
        /// </summary>
        public void PrintList(int pageNo)
        {
            PrintHeader(pageNo);
            Console.ResetColor();
            Utils.PrintBreak('-');
            PrintList(2, 2, pageNo);//bottom 2 because line br:
            Utils.PrintBreak('-');
            Console.WriteLine(this.desc);
        }

        /// <summary>
        /// Prints the list at the current page number
        /// </summary>
        public void PrintList()
        {
            PrintList(this.curPageNo);
        }
        //        /// <summary>
        //		/// Prints the list at the current page number
        //		/// </summary>
        //        public void PrintList(string text)
        //        {
        //            PrintList(text, this.curPageNo);
        //        }

        public void Reset()
        {
            this.curPageNo = 1;
            this.items.Clear();
            this.title = "list";
        }

        private void PrintHeader(int pageNo)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  no |  " + this.title + " - page " + pageNo + "/" + this.pageCount);
        }

        /// <summary>
        /// Prints the list. NB: pageNo is 1-based. It starts with a 1 not a 0! This constructor is the same as (text, pageNo) except you can also specify rel sizes. A relOffBottom of -1 would, for example, limit the lists's height by 1 row.
        /// </summary>
        public void PrintList(string text, int pageNo, int relOffTop, int relOffBottom)
        {
            Console.WriteLine(this.title + " - page " + pageNo + "/" + items.Count / (Console.WindowHeight - 3));
            PrintList(1 + relOffTop, 2 + relOffBottom, pageNo);//bottom 2 because line br:
            Console.WriteLine(text);
            Utils.PrintBreak('-');
        }
    }
}

