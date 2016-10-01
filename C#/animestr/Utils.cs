using System;

namespace animestr
{
	public static class Utils
	{
		public static void PrintBreak(char symbol)
		{
			for (int i = 0; i < Console.WindowWidth; i++) 
			{
				Console.Write (symbol);
			}
			Console.Write (Environment.NewLine);
		}

		public static void ClearConsole()
		{
			for (int i = 0; i < Console.WindowHeight; i++) 
			{
				Console.WriteLine ();
			}
            ConsoleColor store = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
			Console.WriteLine(((char)27)+"c");// = \ec = clear (on some terminals)
            Console.ForegroundColor = store;
		}

        /// <summary>
        /// Performs the ROT13 character rotation.
        /// </summary>
        public static string ROT13(string value)
        {
            char[] array = value.ToCharArray();
            for (int i = 0; i < array.Length; i++)
            {
                int number = (int)array[i];

                if (number >= 'a' && number <= 'z')
                {
                    if (number > 'm')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                else if (number >= 'A' && number <= 'Z')
                {
                    if (number > 'M')
                    {
                        number -= 13;
                    }
                    else
                    {
                        number += 13;
                    }
                }
                array[i] = (char)number;
            }
            return new string(array);
        }
    }
}

