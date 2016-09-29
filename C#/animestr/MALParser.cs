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
            return Format(GetBetween(GetSection(this.page, "<h1", @"</h1", "span"), "\"name\">", "</span"));
        }

        public List<string> GetAlts()
        {
            List<string> ret = new List<string>();
            string section = GetSection(this.page, "itles</h2>", "<h2>");
            foreach (string s in GetBetweens(section, "</span>", "</div>"))
            {
                ret.Add(Format(s).Trim());
            }
            return ret;
        }

        public List<string> GetGenres(){
            List<string> ret = new List<string>();
            string section = GetSection(this.page,"<div>","</div>","Genres:");
            foreach(string s in GetBetweens(section,"title=\"","\"")){
                ret.Add(s);
            }
            return ret;
        }

        public string GetScore(){
            string s = GetSection(this.page, "<div", "</div>", "fl-l score");
            int l = s.LastIndexOf('>')+1;
            return s.Substring(l, s.Length - l).Trim();
        }

        public string GetRank(){
            return GetBetween(GetSection(this.page,"<span","</span>","numbers ranked","Ranked","strong"),"<strong>","</strong>");
        }

        public string GetPopularity(){
            return GetBetween(GetSection(this.page,"<span","</span>","numbers popularity","Popularity","strong"),"<strong>","</strong>");
        }

        public string GetDescription()
        {
            return Format(string.Join("", GetBetweens(GetBetween(this.page, "itemprop=\"description\"", "</span"), ">", "<")));
        }

        /// <summary>
        /// For search result pages
        /// </summary>
        public string GetLinkSection()
        {
            return GetSection(this.page, "<a", @"</a", "strong");
        }

        /// <summary>
        /// Gets substring of the text - marked by two strings
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="m1">opening marker</param>
        /// <param name="m2">closing marker</param>
        /// <returns></returns>
        public static string GetBetween(string text, string m1, string m2)
        {
            int oIndex = text.IndexOf(m1) + m1.Length;
            return text.Substring(oIndex, ((text.IndexOf(m2, oIndex) - oIndex) > 0 ? (text.IndexOf(m2, oIndex) - oIndex) : text.Length - oIndex));
        }

        public static string Format(string input){
            return System.Net.WebUtility.HtmlDecode(input);
        }

        /// <summary>
        /// Gets substring of the text - marked by two strings
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="m1">opening marker</param>
        /// <param name="m2">closing marker</param>
        /// <returns></returns>
        public static List<string> GetBetweens(string text, string m1, string m2)
        {
            List<string> ret = new List<string>();
            bool getting = true;
            int offset = 0;
            while (getting)
            {
                int oIndex = text.IndexOf(m1,offset) + m1.Length;
                string s = text.Substring(oIndex, ((text.IndexOf(m2, oIndex) - oIndex) > 0 ? (text.IndexOf(m2, oIndex) - oIndex) : text.Length - oIndex));
                ret.Add(s);

                offset = oIndex + s.Length + m2.Length;
                try{
                getting = text.IndexOf(m1, offset) > 0;
                }catch(ArgumentOutOfRangeException){
                    getting = false;
                }
            }
            return ret;
        }

        /// <summary>
        /// Section selection
        /// </summary>
        /// <param name="tag1">Opening marker. E.g: "&lt;a"</param>
        /// <param name="tag2">Closing marker. E.g: "&lt;/a"</param>
        /// <param name="contain">text which must be within the two markers. Method will keep searching until a section with this text is found</param>
        /// <returns></returns>
        public static string GetSection(string text, string tag1, string tag2, params string[] contains)
        {
            bool found = false;
            int offset = 0;
            string section = null;
            int no = 0;
            while (!found)
            {
                int aIndex = text.IndexOf(tag1, offset);
                if (aIndex < 0)
                {
                    return null;
                }
                try
                {
                    section = text.Substring(aIndex, text.IndexOf(tag2, aIndex) - aIndex);

                    bool containsAll = true;

                    foreach (string contain in contains)
                    {
                        if (!section.Contains(contain))
                        {
                            containsAll = false;
                        }
                    }

                    if (containsAll)
                    {
                        found = true;
                    }
                    else
                    {
                        offset = aIndex + tag1.Length;
                    }
                }
                catch
                {
                    return null;
                }

                if (no > text.Length)
                {
                    return null; //could not find section in the page
                }

                no++;
            }
            return section;
        }
    }
}
