﻿using System;

namespace animestr
{
    public class EpisodesView
    {
        public Display display = null;
        public EpisodeData data = null;

        public EpisodesView(Display display, EpisodeData data)
        {
            this.display = display;
            this.data = data;
        }
    }
}

