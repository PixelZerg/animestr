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

        /// <summary>
        /// On "search result" pages
        /// </summary>
        public string GetLinkSection()
        {
            bool linkSectionFound = false;
            int offset = 0;
            string linkSection = null;
            while (!linkSectionFound)
            {
                int aIndex = page.IndexOf("<a", offset);
                if (aIndex < 0)
                {
                    return null;
                }
                try
                {
                    linkSection = page.Substring(aIndex, page.IndexOf(@"</a", aIndex) - aIndex);

                    if (linkSection.Contains("strong"))
                    {
                        linkSectionFound = true;
                    }
                    else
                    {
                        offset = aIndex + 2; //2 = "<a" length
                    }
                }
                catch
                {
                    return null;
                }
            }
            return linkSection;
        }
    }
}
