using System;

namespace DotScrape.Attributes.Formatters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SubstringAttribute : TransformAttribute
    {
        public SubstringAttribute(int startIndex = -1, int length = -1)
        {
            StartIndex = startIndex;
            Length = length;
        }

        public int StartIndex { get; }
        public int Length { get; }

        public override string Visit(string stringData, ScrapeHtmlNode node)
        {
            var startIndex = StartIndex >= 0 ? StartIndex : 0;
            var length = Length >= 0 ? Length : ((node.InnerText?.Length ?? 0) - startIndex);

            stringData = stringData.Substring(startIndex, length);

            return stringData;
        }
    }
}
