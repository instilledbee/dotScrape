using System;

namespace DotScrape.Attributes.Formatters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TrimStringAttribute : TransformAttribute
    {
        public override string Visit(string stringData, ScrapeHtmlNode node)
        {
            stringData = stringData?.Trim();
            return stringData;
        }
    }
}
