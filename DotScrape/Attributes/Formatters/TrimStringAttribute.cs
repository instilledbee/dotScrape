using System;

namespace DotScrape.Attributes.Formatters
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TrimStringAttribute : Attribute
    {
    }
}
