using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class Display
    {
        public Display()
        {

        }

        public void Show()
        {
            DisplaySplash("Loading recommendations...");
        }

        public void DisplaySplash(string text)
        {
            Utils.ClearConsole();
            if (Console.WindowHeight > 8 && Console.WindowWidth > 52)
            {
                int no = 0;
                while (true)
                {
                    string[][] frames = (no % 2 == 0 ? Consts.ASCII_TEXT_FRAMES.Reverse().ToArray() : Consts.ASCII_TEXT_FRAMES);
                    foreach (string[] frame in frames)
                    {
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

                        System.Threading.Thread.Sleep(200);
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
            }
            Console.ResetColor();

        }

    }
}
