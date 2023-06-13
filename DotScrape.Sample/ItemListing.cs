using DotScrape.Attributes.Formatters;
using DotScrape.Attributes.Selectors;

namespace DotScrape.Sample
{
    internal class ItemListing
    {
        [ViaCss("table a.item-name")]
        [ViaXPath(@"table//a[@class=""item-name""]")]
        public string? ItemName { get; set; }

        [ViaXPath("a")]
        public string? Username { get; set; }

        [ViaCss(".catprice h3")]
        [Substring(1)]
        public decimal Price { get; set; }

        public DateTime DatePosted { get; set; }
    }

    internal class ItemsForSale
    {
        [ViaCss("ul#item-search-results li")]
        public List<ItemListing> Items { get; set; }
    }
}
