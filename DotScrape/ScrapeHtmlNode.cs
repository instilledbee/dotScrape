using System;
using System.Collections.Generic;
using System.Text;

namespace DotScrape
{
    /// <summary>
    /// Standardizes representation of HTML nodes within DotScrape
    /// </summary>
    public class ScrapeHtmlNode
    {
        public IEnumerable<ScrapeHtmlNodeAttribute> Attributes { get; set; }
        public string InnerText { get; set; }
    }

    /// <summary>
    /// Standardizes representation of HTML node attributes within DotScrape
    /// </summary>
    public class ScrapeHtmlNodeAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
