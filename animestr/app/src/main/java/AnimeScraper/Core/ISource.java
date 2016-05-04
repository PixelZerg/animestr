package AnimeScraper.Core;
import java.util.*;

public interface ISource {
    public List<AnimeData> Search(String query, Boolean justnames);
    public void ParseEpisodes(AnimeData anime);
}
