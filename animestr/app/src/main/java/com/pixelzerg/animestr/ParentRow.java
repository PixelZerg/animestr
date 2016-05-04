package com.pixelzerg.animestr;

import java.util.*;

import AnimeScraper.Core.AnimeData;

/**
 * Created by PixelZerg on 16/04/2016.
 */
public class ParentRow {
    private String name;
    private List<AnimeData> childlist;

    public ParentRow(String name, List<AnimeData> childlist) {
        this.name = name;
        this.childlist = childlist;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<AnimeData> getChildlist() {
        return childlist;
    }

    public void setChildlist(ArrayList<AnimeData> childlist) {
        this.childlist = childlist;
    }
}
