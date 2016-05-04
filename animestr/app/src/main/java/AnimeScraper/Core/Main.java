package AnimeScraper.Core;

import java.util.ArrayList;
import java.util.List;

public class Main {
    public static List<ISource> scrapers = new ArrayList<ISource>();
    public static void LoadScrapers(){
        scrapers.add(new AnimeScraper.Scrapers.AnimeDaoScraper());
    }
}
