using System;

namespace animestr
{
	public static class Utils
	{
		/// <summary>
		/// Prints the list. NB: pageNo is 1-based. It starts with a 1 not a 0!
		/// </summary>
		/// <param name="offtop">Offtop.</param>
		/// <param name="offbottom">Offbottom.</param>
		/// <param name="pageNo">Page no.</param>
		/// <param name="items">Items.</param>
		public static void PrintList(int offtop, int offbottom, int pageNo, params string[] items)
		{	
			offbottom += 1; //padding

			int pageSize = (Console.WindowHeight - offtop - offbottom);//size = amount of elements 
			for (int i = pageSize * (pageNo - 1); i < ((pageSize * (pageNo - 1))+pageSize); i++) 
			{
				if (i % 2 == 0) 
				{
					Console.ForegroundColor = ConsoleColor.White;
				}
				if (i < items.Length) {
					Console.WriteLine ((i < 9 ? " " : "") + (i + 1) + ") " + (items[i].Length>=Console.WindowWidth-8-i.ToString().Length ? items[i].Substring(0,Console.WindowWidth-8-i.ToString().Length)+"...": items[i]));
				} else {
					Console.WriteLine ();//padding
				}
				Console.ResetColor ();
			}
				
		}

		public static void PrintList(int pageNo, params string[] items)
		{
			PrintList (0, 0, pageNo, items);
		}

		public static void PrintList(string title, string text, int pageNo, params string[] items)
		{
			Console.WriteLine (title);
			PrintList (1, 2, pageNo, items);//bottom 2 because line br:
			Console.WriteLine (text);
			PrintBreak ('-');
		}

		public static void PrintList(string title, int pageNo, params string[] items)
		{
			Console.WriteLine (title);
			PrintList (1, 0, pageNo, items);
		}

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
		}
	}
}

