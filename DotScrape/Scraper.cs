using System;

namespace DotScrape
{

    public interface IScraper 
    {
        TModel Scrape<TModel>() where TModel : new();
    }
}
