using System;

namespace DotScrape
{

    public interface IScraper { }

    public interface IScraper<TModel> : IScraper
        where TModel: new()
    {
        TModel Parse();
    }
}
