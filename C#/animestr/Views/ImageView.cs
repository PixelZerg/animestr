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
        public ColourMode ColourMode = ColourMode.ANSI256;

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
                    for (int x = 0; x < bmpWidth && x < boundWidth; x++)
                    {
                        int b = currentPixel[x * 3 + 0];
                        int g = currentPixel[x * 3 + 1];
                        int r = currentPixel[x * 3 + 2];
                        //int alpha = currentPixel[x * 4 + 3];
                        SetColour(r, g, b);
                        Console.Write(GetSymbol(r, g, b));
                    }
                    currentPixel += bmpStride;
                    Console.Write(Environment.NewLine);
                }
            }
            ResetColour();
            scaled.UnlockBits(bmpData);
        }

        public void SetColour(int r, int g, int b)
        {
            switch (ColourMode)
            {
                case ColourMode.ANSI256:
                    Console.Write("\x1b[38;5;" + BashColour.ClosestBash(Color.FromArgb(r, g, b)) + "m");
                    break;
            }
        }

        public static string symbols = @"@%#Xx+";

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

        public void ResetColour()
        {
            switch (ColourMode)
            {
                case ColourMode.ANSI256:
                    Console.Write("\x1b[0m");
                    break;
            }
        }


    }
}

