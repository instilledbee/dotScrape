using System;

namespace DotScrape.Attributes.Selectors
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ViaCssAttribute : Attribute
    {
        public ViaCssAttribute(string cssSelector)
        {
            CssSelector = cssSelector;
        }

        public string CssSelector { get; }
    }
}
