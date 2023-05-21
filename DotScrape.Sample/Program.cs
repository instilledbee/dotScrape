using dotScrape.POC;
using HtmlAgilityPack;
using System.Text.Json;

namespace dotScrape.Sample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var htmlDoc = new HtmlWeb().Load(@"https://tipidpc.com/catalog.php?cat=4&sec=s");
            var scraper = new HtmlAgilityScraper<ItemForSale>(htmlDoc);

            var data = scraper.Parse();

            Console.WriteLine(JsonSerializer.Serialize(data));
        }
    }
}