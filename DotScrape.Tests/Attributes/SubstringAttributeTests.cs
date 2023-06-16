using DotScrape.Attributes.Formatters;

namespace DotScrape.Tests.Attributes
{
    public class SubstringAttributeTests
    {
        [Test]
        public void SubstringAttribute_ReturnsSubString_StartingFromStartIndex()
        {
            // arrange
            var htmlNode = new ScrapeHtmlNode()
            {
                InnerText = "Hello World!"
            };

            var substringAttribute = new SubstringAttribute(6);
            string stringData = htmlNode.InnerText;

            // act
            stringData = substringAttribute.Visit(stringData, htmlNode);

            // assert
            Assert.That(stringData, Is.EqualTo("World!"));
        }

        [Test]
        public void SubstringAttribute_ReturnsSubString_WithCorrectLength()
        {
            // arrange
            var htmlNode = new ScrapeHtmlNode()
            {
                InnerText = "Hello World!"
            };

            var substringAttribute = new SubstringAttribute(0, 5);
            string stringData = htmlNode.InnerText;

            // act
            stringData = substringAttribute.Visit(stringData, htmlNode);

            // assert
            Assert.That(stringData, Is.EqualTo("Hello"));
        }


        [Test]
        public void SubstringAttribute_ReturnsSubString_WithCorrectStartIndexAndLength()
        {
            // arrange
            var htmlNode = new ScrapeHtmlNode()
            {
                InnerText = "Hello World!"
            };

            var substringAttribute = new SubstringAttribute(1, 4);
            string stringData = htmlNode.InnerText;

            // act
            stringData = substringAttribute.Visit(stringData, htmlNode);

            // assert
            Assert.That(stringData, Is.EqualTo("ello"));
        }
    }
}
