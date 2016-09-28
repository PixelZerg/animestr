package AnimeScraper.Core;

import java.io.Serializable;
import java.util.*;

public class AnimeData implements Serializable{
    public String Title = "Untitled";
    public String Alternative = "";
    public String Description = "...";
    public int EpisodeCount = 0;
    public String CoverUrl = "";
    public String SourceUrl = "";

    public List<EpisodeData> Episodes = new ArrayList<EpisodeData>();

    @Override
    public String toString(){
        return "{"+"Title=\""+Title+"\","+
                "Alt=\""+Alternative+"\","+
                "Desc=\""+Description+"\","+
                "EpisodeCount=\""+EpisodeCount+"\","+
                "Cover=\""+CoverUrl+"\","+
                "Source=\""+SourceUrl+"\","+"}";
        //TODO FIX
    }

}
