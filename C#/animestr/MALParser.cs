using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace animestr
{
    public class MALParser
    {
        public string page = null;

        public MALParser(string page)
        {
            this.page = page;
        }

        public string GetTitle()
        {
            return Parsing.Format(Parsing.GetBetween(Parsing.GetSection(this.page, "<h1", @"</h1", "span"), "\"name\">", "</span"));
        }

        public List<string> GetAlts()
        {
            List<string> ret = new List<string>();
            string section = Parsing.GetSection(this.page, "itles</h2>", "<h2>");
            foreach (string s in Parsing.GetBetweens(section, "</span>", "</div>"))
            {
                ret.Add(Parsing.Format(s).Trim());
            }
            return ret;
        }

        public List<string> GetGenres(){
            List<string> ret = new List<string>();
            string section = Parsing.GetSection(this.page,"<div>","</div>","Genres:");
            foreach(string s in Parsing.GetBetweens(section,"title=\"","\"")){
                ret.Add(s);
            }
            return ret;
        }

        public string GetScore(){
            string s = Parsing.GetSection(this.page, "<div", "</div>", "fl-l score");
            int l = s.LastIndexOf('>')+1;
            return s.Substring(l, s.Length - l).Trim();
        }

        public string GetRank(){
            return Parsing.GetBetween(Parsing.GetSection(this.page,"<span","</span>","numbers ranked","Ranked","strong"),"<strong>","</strong>");
        }

        public string GetPopularity(){
            return Parsing.GetBetween(Parsing.GetSection(this.page,"<span","</span>","numbers popularity","Popularity","strong"),"<strong>","</strong>");
        }

        public string GetDescription()
        {
            return Parsing.Format(string.Join("", Parsing.GetBetweens(Parsing.GetBetween(this.page, "itemprop=\"description\"", "</span"), ">", "<")));
        }

        /// <summary>
        /// For search result pages
        /// </summary>
        public string GetLinkSection()
        {
            return Parsing.GetSection(this.page, "<a", @"</a", "strong");
        }
    }
}
