using DotScrape.Attributes.Formatters;

namespace DotScrape.Tests.Attributes
{
    public class TrimStringAttributeTests
    {
        [Test]
        public void TrimStringAttribute_TrimsInnerText()
        {
            // arrange
            var htmlNode = new ScrapeHtmlNode()
            {
                InnerText = "\r\n  Hello World!  "
            };

            var trimStringAttribute = new TrimStringAttribute();
            string stringData = htmlNode.InnerText;

            // act
            stringData = trimStringAttribute.Visit(stringData, htmlNode);

            // assert
            Assert.That(stringData, Is.EqualTo("Hello World!"));
        }
    }
}
