using System;
using System.Drawing;
using System.Net;
using System.IO;

namespace animestr
{
	public class AnimeEntry
	{
		public string name = "unkown";
		public Uri pictureUrl = null;
        public Uri url = null;

		private Bitmap bmp = null;

		public AnimeEntry()
		{
		}

		public AnimeEntry(string name, Uri url, Uri picUrl)
		{
            this.url = url;
			this.name = name;
			this.pictureUrl = picUrl;
		}


		public Bitmap GetBitmap()
		{
			if (bmp == null) {
				using (WebClient wc = new WebClient())
				{
					using (Stream s = wc.OpenRead(pictureUrl))
					{
						bmp = new Bitmap (s);
					}
				}
			}

			return bmp;
		}

        public override string ToString()
        {
            return "Anime Entry =>" + Environment.NewLine
                  + "\tName: " + this.name + Environment.NewLine
                  + "\tUrl: " + this.url + Environment.NewLine
                  + "\tPicture: " + this.pictureUrl;
        }
    }
}

