package AnimeScraper.Scrapers;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import AnimeScraper.Core.AnimeData;
import AnimeScraper.Core.EpisodeData;
import AnimeScraper.Core.ISource;

import org.jsoup.Jsoup;
import org.jsoup.nodes.*;
import org.jsoup.select.Elements;

public class AnimeDaoScraper implements ISource {
    public static Pattern AltRgx = Pattern.compile("Alternative:<\\/b>(.|\\s|.)+?<br>");
    public static Pattern DescRgx = Pattern.compile("Description<\\/b>(.|\\s)+<br>");
    public static Pattern s720pRgx = Pattern.compile("var\\s+s720p\\s+=(.|\\s)+?;");

    @Override
    public List<AnimeData> Search(String query, Boolean justnames) {
        List<AnimeData> ret = new ArrayList<AnimeData>();
        try {
            Document doc = Jsoup.connect("http://animedao.com/search/"+query).userAgent("Mozilla").get();
            Elements links = doc.select(".latestanime-title a");
            for(int i = 0; i <links.size();i++){
                if(!justnames){
                    ret.add(ScrapeAnimeData("http://animedao.com"+links.get(i).attr("href")));
                }else{
                    AnimeData ad = new AnimeData();
                    ad.Title = links.get(i).text();
                    ad.CoverUrl = "http://animedao.com"+links.get(i).parent().parent().parent().child(0).child(0).child(0).child(0).attr("data-original");
                    ad.SourceUrl = "http://animedao.com"+links.get(i).attr("href");
                    ret.add(ad);
                }
            }

        } catch (IOException e) {
            e.printStackTrace();
        }
        return ret;
    }

    public static String rot13(String input) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < input.length(); i++) {
            char c = input.charAt(i);
            if       (c >= 'a' && c <= 'm') c += 13;
            else if  (c >= 'A' && c <= 'M') c += 13;
            else if  (c >= 'n' && c <= 'z') c -= 13;
            else if  (c >= 'N' && c <= 'Z') c -= 13;
            sb.append(c);
        }
        return sb.toString();
    }

    @Override
    public void ParseEpisodes(AnimeData anime) {
        try {
            Document listdoc = Jsoup.connect(anime.SourceUrl).userAgent("Mozilla").get();
            Elements episodeElem = listdoc.select(".animeinfo-content");
            for(int i =0; i < episodeElem.size(); i++){
                EpisodeData ed = new EpisodeData();
                ed.EpisodeNo = i+1;
                ed.Title = episodeElem.get(i).select("p").get(0).text()+" - "+episodeElem.get(i).select("p").get(1).text();

                //and now for the main bit
                Document vidDoc = Jsoup.connect("http://animedao.com"+episodeElem.get(i).attr("href")).userAgent("Mozilla").get();
                //ed.VideoUrl = vidDoc.html();
                String coderaw = vidDoc.select("body div.container script").get(1).html();

                Matcher m = s720pRgx.matcher(coderaw);
                m.find();
                String code = m.group();
                code = code.substring(code.indexOf("\"") + 1);
                code = code.substring(0, code.indexOf("\"")-1);
                code = rot13(code);

                ed.VideoUrl = code;
                ed.VideoUrl = "http://animedao.com/redirect/"+code;

                anime.Episodes.add(ed);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public AnimeData ScrapeAnimeData(String Url){
        AnimeData ret = new AnimeData();
        try{
            Document doc = Jsoup.connect(Url).userAgent("Mozilla").get();
            ret.SourceUrl = Url;
            Element Meta = doc.select(".col-md-9").first();
            ret.Title = Meta.select("h2").first().text();
            //ret.Alternative = Meta.html();
            Matcher m = AltRgx.matcher(Meta.html());
            m.find();
            String altraw = m.group();
            altraw = altraw.substring(altraw.indexOf(">") + 1);
            altraw = altraw.substring(0, altraw.indexOf("<")-1);
            ret.Alternative = altraw;

            Matcher mm = DescRgx.matcher(Meta.html());
            mm.find();
            String descraw = mm.group();
            descraw = descraw.substring(descraw.indexOf("<br>") + 4);
            descraw = descraw.substring(0, descraw.lastIndexOf("<br>")-7);
            ret.Description = descraw;

            ret.CoverUrl = "http://animedao.com"+doc.select(".col-sm-3 center img").attr("src");

            ret.EpisodeCount = doc.select(".animeinfo-content").size();
        }catch(Exception e){e.printStackTrace();}

        return ret;

    }

}
