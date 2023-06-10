using System;

namespace DotScrape.Attributes.Extractors
{
    public class FromAttributeAttribute : Attribute
    {
        public string AttributeName { get; set; }

        public FromAttributeAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }
    }
}
