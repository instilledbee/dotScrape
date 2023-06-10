using DotScrape.Attributes.Extractors;
using DotScrape.Attributes.Formatters;
using DotScrape.Attributes.Selectors;
using DotScrape.HtmlAgilityPack;
using HtmlAgilityPack;

namespace DotScrape.Tests
{
    public class HtmlAgilityScraperTests
    {
        private HtmlDocument _htmlDoc;

        [SetUp]
        public void Setup()
        {
            _htmlDoc = new HtmlDocument();
            
            using (var reader = File.OpenRead(@"TestData/github.html"))
            {
                _htmlDoc.Load(reader);
            }
        }

        [Test]
        public void HtmlAgilityScraper_PopulatesElementsOfSingleModel_UsingCssSelectors()
        {
            var scraper = new HtmlAgilityScraper(_htmlDoc);
            var model = scraper.Scrape<ProfileInfoViaCss>();

            Assert.IsNotNull(model);
            Assert.That(model.DisplayName, Is.EqualTo("Junvic"));
            Assert.That(model.Username, Is.EqualTo("instilledbee"));
            Assert.That(model.Followers, Is.EqualTo(10));
            Assert.That(model.Following, Is.EqualTo(19));
            Assert.That(model.ProfilePicUrl, Is.EqualTo(@"https://avatars.githubusercontent.com/u/5665389?v=4"));
        }

        [Test]
        public void HtmlAgilityScraper_PopulatesElementsOfSingleModel_UsingXPath()
        {
            var scraper = new HtmlAgilityScraper(_htmlDoc);
            var model = scraper.Scrape<ProfileInfoViaXPath>();

            Assert.IsNotNull(model);
            Assert.That(model.DisplayName, Is.EqualTo("Junvic"));
            Assert.That(model.Username, Is.EqualTo("instilledbee"));
            Assert.That(model.Followers, Is.EqualTo(10));
            Assert.That(model.Following, Is.EqualTo(19));
            Assert.That(model.ProfilePicUrl, Is.EqualTo(@"https://avatars.githubusercontent.com/u/5665389?v=4"));
        }
    }

    internal class ProfileInfoViaCss
    {
        [ViaCss("h1.vcard-names span.p-name")]
        [TrimString]
        public string? DisplayName { get; set; }

        [ViaCss("h1.vcard-names span.p-nickname")]
        [TrimString]
        public string? Username { get; set; }

        [ViaCss("div.flex-order-1 a:nth-child(1) > span.text-bold")]
        public int? Followers { get; set; }

        [ViaCss("div.flex-order-1 a:nth-child(2) > span.text-bold")]
        public int? Following { get; set; }

        [ViaCss("img.avatar.avatar-user.width-full")]
        [FromAttribute("src")]
        public string? ProfilePicUrl { get; set; }
    }

    internal class ProfileInfoViaXPath
    {
        [ViaXPath(@"//h1[@class='vcard-names']//span[contains(concat(' ',normalize-space(@class),' '),'p-name')]")]
        [TrimString]
        public string? DisplayName { get; set; }

        [ViaXPath(@"//h1[@class='vcard-names']//span[contains(concat(' ',normalize-space(@class),' '),'p-nickname')]")]
        [TrimString]
        public string? Username { get; set; }

        [ViaXPath(@"//div[contains(concat(' ',normalize-space(@class),' '),'flex-order-1')]//a[1]//span[contains(concat(' ',normalize-space(@class),' '),'text-bold')]")]
        public int? Followers { get; set; }

        [ViaXPath(@"//div[contains(concat(' ',normalize-space(@class),' '),'flex-order-1')]//a[2]//span[contains(concat(' ',normalize-space(@class),' '),'text-bold')]")]
        public int? Following { get; set; }

        [ViaXPath(@"//img[contains(concat(' ',normalize-space(@class),' '),'avatar avatar-user')]")]
        [FromAttribute("src")]
        public string? ProfilePicUrl { get; set; }
    }

    internal class RepositoryInfo
    {
        [ViaCss("ul > li h3 a")]
        public string? Name { get; set; }

        [ViaCss("ul > li span.ml-0.mr-3 span:nth-child(2)")]
        public string? PrimaryLanguage { get; set; }

        [ViaCss("ul > li relative-time")]
        [FromAttribute("datetime")]
        public DateTime? LastUpdated { get; set; }
    }
}