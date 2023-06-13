using HtmlAgilityPack;
using System.Linq;

namespace DotScrape.HtmlAgilityPack
{
    public static class HtmlNodeExtensions
    {
        public static ScrapeHtmlNode ToScrapeHtmlNode(this HtmlNode node)
        {
            return new ScrapeHtmlNode()
            {
                InnerText = node.InnerText,
                Attributes = node.Attributes.Select(n => new ScrapeHtmlNodeAttribute()
                {
                    Name = n.Name,
                    Value = n.Value
                })
            };
        }
    }
}
