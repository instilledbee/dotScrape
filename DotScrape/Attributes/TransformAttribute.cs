using System;

namespace DotScrape.Attributes
{
    public abstract class TransformAttribute : Attribute
    {
        public abstract string Visit(string stringData, ScrapeHtmlNode node);
    }
}
