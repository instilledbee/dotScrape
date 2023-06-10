using System;

namespace DotScrape.Attributes.Selectors
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ViaXPathAttribute : Attribute
    {
        public ViaXPathAttribute(string xpath)
        {
            Xpath = xpath;
        }

        public string Xpath { get; }
    }
}
