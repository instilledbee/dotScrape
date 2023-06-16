using DotScrape.Attributes.Extractors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotScrape.Tests.Attributes
{
    public class FromAttributeAttributeTests
    {
        [Test]
        public void FromAttributeAttribute_SelectsCorrectAttribute()
        {
            // arrange
            var htmlNode = new ScrapeHtmlNode()
            {
                Attributes = new List<ScrapeHtmlNodeAttribute>()
                {
                    new ScrapeHtmlNodeAttribute() { Name = "name", Value = "InstilledBee" },
                    new ScrapeHtmlNodeAttribute() { Name = "date", Value = DateTime.Today.ToString() }
                }
            };

            var fromAttribute = new FromAttributeAttribute("name");
            string stringData = string.Empty;

            // act
            stringData = fromAttribute.Visit(stringData, htmlNode);

            // assert
            Assert.That(stringData, Is.EqualTo("InstilledBee"));
        }
    }
}
