using System;

namespace DotScrape.StringFormatAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class SubstringAttribute : Attribute, IStringFormatAttribute
    {
        public SubstringAttribute(int startIndex = -1, int endIndex = -1)
        {
            StartIndex = startIndex;
            Length = endIndex;
        }

        public int StartIndex { get; }
        public int Length { get; }
    }
}
