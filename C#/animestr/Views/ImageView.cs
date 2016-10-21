using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace animestr
{
    public enum ColourMode
    {
        ANSI16,
        ANSI256,
        TrueColour,
        Plain,
    }

    public class ImageView
    {
        public Bitmap bmp = null;
        public ColourMode ColourMode = ColourMode.Plain;
        public bool background = false;

        public ImageView()
        {
        }

        public ImageView(Bitmap bmp)
        {
            this.bmp = bmp;
        }

        public void Show()
        {
            Console.WriteLine();
            PrintPicture(Console.WindowWidth, Console.WindowHeight - 1);
            Console.ReadKey();
        }

        public void PrintPicture(int boundWidth, int boundHeight)
        {
            this.ResetColour();

            Bitmap scaled = new Bitmap(bmp, (bmp.Width / (bmp.Height / boundHeight)) * 2, boundHeight);
            int padding = (boundWidth - scaled.Width) / 2;

            int xoff = (padding >= 0) ? 0 : (padding * -1) / 2;

            int bmpHeight = scaled.Height;
            int bmpWidth = scaled.Width;

            BitmapData bmpData = scaled.LockBits(new Rectangle(0, 0, bmpWidth, bmpHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            unsafe
            {
                int bmpStride = bmpData.Stride;
                byte* currentPixel = (byte*)bmpData.Scan0;

                for (int y = 0; y < bmpHeight; y++)
                {
                    for (int i = 0; i < padding; i++)
                    {
                        Console.Write(" ");
                    }
                    for (int x = xoff; x < bmpWidth && x < boundWidth; x++)
                    {
                        int b = currentPixel[x * 3 + 0];
                        int g = currentPixel[x * 3 + 1];
                        int r = currentPixel[x * 3 + 2];
                        //int alpha = currentPixel[x * 4 + 3];
                        SetColour(r, g, b);
                        if (!background)
                        {
                            Console.Write(GetSymbol(r, g, b));
                        }
                        else
                        {
                            Console.Write(' ');
                        }
                    }
                    currentPixel += bmpStride;
                    //Console.Write("\x1b[0m");
                    //Console.ResetColor();
                    this.ResetColour();
                    Console.Write(Environment.NewLine);
                }
            }
            ResetColour();
            scaled.UnlockBits(bmpData);
        }

        public void ResetColour()
        {
            if (this.ColourMode == ColourMode.Plain || this.ColourMode == ColourMode.ANSI16)
            {
                Console.ResetColor();
            }
            else
            {
                Console.Write("\x1b[0m");
            }
        }

        public void SetColour(int r, int g, int b)
        {
            switch (this.ColourMode)
            {
                case ColourMode.ANSI256:
                    Console.Write("\x1b[" + (this.background ? "48" : "38") + ";5;" + BashColour.ClosestBash(Color.FromArgb(r, g, b)) + "m");
                    break;
                case ColourMode.TrueColour:
                    Console.Write("\x1b[" + (this.background ? "48" : "38") + ";2;" + r + ";" + g + ";" + b + "m");
                    break;
                case ColourMode.ANSI16:
                    if (this.background)
                    {
                        Console.BackgroundColor = (ConsoleColor)BashColour.ClosestBash16(Color.FromArgb(b, g, r));
                    }
                    else
                    {
                        Console.ForegroundColor = (ConsoleColor)BashColour.ClosestBash16(Color.FromArgb(b, g, r));
                    }
                    break;
                case ColourMode.Plain:
                    break;
            }
        }

        //public static string symbols = @"@%#Xx+";
        public static string symbols = @"+xX#%@";

        public char GetSymbol(int r, int g, int b)
        {
            double lightness = (Math.Abs(r)) + (Math.Abs(b)) + (Math.Abs(g));
            //0 is very dark
            for (int i = 0; i < symbols.Length; i++)
            {
                if (lightness <= (765 / ((float)symbols.Length)) * (i + 1))
                {
                    return symbols[i];
                }
            }
            //return '.';
            return ' ';
        }

    }
}

