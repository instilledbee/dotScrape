using DotScrape.SelectorAttributes;
using DotScrape.StringFormatAttributes;

namespace dotScrape.Sample
{
    internal class ItemForSale
    {
        [ViaCss("ul#item-search-results li:nth-child(1) a.item-name")]
        [ViaXPath(@"//ul[@id=""item-search-results""]//li[1]//a[@class=""item-name""]")]
        public string? ItemName { get; set; }

        [ViaCss("ul#item-search-results li:nth-child(1) > a")]
        public string? Username { get; set; }

        [ViaCss("ul#item-search-results li:nth-child(1) .catprice h3")]
        [Substring(1)]
        public decimal Price { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
