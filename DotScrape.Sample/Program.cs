using DotScrape.HtmlAgilityPack;
using HtmlAgilityPack;
using System.Text.Json;

namespace DotScrape.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var htmlDoc = new HtmlWeb().Load(@"https://tipidpc.com/catalog.php?cat=4&sec=s");
            var scraper = new HtmlAgilityScraper(htmlDoc);

            var data = scraper.Scrape<ItemsForSale>();

            Console.WriteLine(JsonSerializer.Serialize(data));
        }
    }
}