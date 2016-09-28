using System;
using System.Drawing;
using System.Net;
using System.IO;

namespace animestr
{
	public class AnimeData
	{
		public string name = "unkown";
		public Uri pictureUrl = null;
		private Bitmap bmp = null;

		public AnimeData()
		{
		}

		public AnimeData(string name, Uri picUrl)
		{
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
	}
}

