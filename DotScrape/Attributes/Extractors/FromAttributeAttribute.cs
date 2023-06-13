using System;
using System.Linq;

namespace DotScrape.Attributes.Extractors
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class FromAttributeAttribute : TransformAttribute
    {
        public string AttributeName { get; set; }

        public FromAttributeAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        public override string Visit(string stringData, ScrapeHtmlNode node)
        {
            var elementAttribute = node.Attributes.FirstOrDefault(a => a.Name == AttributeName);

            if (elementAttribute != null)
                return elementAttribute.Value;

            return stringData;
        }
    }
}
