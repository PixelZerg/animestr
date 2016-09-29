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
			for (int i = 0; i < Console.WindowHeight*2; i++) 
			{
				Console.WriteLine ();
			}
			Console.WriteLine(((char)27)+"c");// = \ec = clear (on some terminals)
		}
	}
}

