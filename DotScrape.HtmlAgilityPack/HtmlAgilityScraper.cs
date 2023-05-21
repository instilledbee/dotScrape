using DotScrape;
using DotScrape.SelectorAttributes;
using DotScrape.StringFormatAttributes;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace dotScrape.POC
{
    public class HtmlAgilityScraper<TModel> : IScraper<TModel>
        where TModel : new()
    {
        private readonly HtmlDocument _htmlDocument;

        public HtmlAgilityScraper(HtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        public Uri BaseUrl { get; }

        public TModel Parse()
        {
            var result = new TModel();

            MapPropertiesWithCssSelectors(result);
            MapPropertiesWithXPathSelectors(result);

            return result;
        }

        private void MapPropertiesWithXPathSelectors(TModel result)
        {
            var mappableXPathProps = result.GetType()
                                         .GetProperties()
                                         .Where(p => Attribute.IsDefined(p, typeof(ViaXPathAttribute)));

            if (mappableXPathProps.Any())
            {
                foreach (var prop in mappableXPathProps)
                {
                    var attr = prop.GetCustomAttribute<ViaXPathAttribute>();
                    var selector = attr.Xpath;

                    var node = _htmlDocument.DocumentNode.SelectSingleNode(selector);
                    var stringData = node?.InnerText;

                    var substringAttribute = prop.GetCustomAttribute<SubstringAttribute>();

                    if (substringAttribute != null)
                    {
                        var startIndex = substringAttribute.StartIndex >= 0 ? substringAttribute.StartIndex : 0;
                        var length = substringAttribute.Length >= 0 ? substringAttribute.Length : (stringData.Length - startIndex);

                        stringData = stringData.Substring(startIndex, length);
                    }

                    if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(result, stringData);
                        continue;
                    }

                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);

                    if (converter != null)
                    {
                        var convertedValue = converter.ConvertFrom(stringData);
                        prop.SetValue(result, convertedValue);
                    }
                }
            }
        }

        private void MapPropertiesWithCssSelectors(TModel result)
        {
            var mappableCssProps = result.GetType()
                                         .GetProperties()
                                         .Where(p => Attribute.IsDefined(p, typeof(ViaCssAttribute)));

            if (mappableCssProps.Any())
            {
                foreach (var prop in mappableCssProps)
                {
                    var attr = prop.GetCustomAttribute<ViaCssAttribute>();
                    var selector = attr.CssSelector;

                    var node = _htmlDocument.DocumentNode.QuerySelectorAll(selector).FirstOrDefault();
                    var stringData = node?.InnerText;

                    var substringAttribute = prop.GetCustomAttribute<SubstringAttribute>();

                    if (substringAttribute != null)
                    {
                        var startIndex = substringAttribute.StartIndex >= 0 ? substringAttribute.StartIndex : 0;
                        var length = substringAttribute.Length >= 0 ? substringAttribute.Length : (stringData.Length - startIndex);

                        stringData = stringData.Substring(startIndex, length);
                    }

                    if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(result, stringData);
                        continue;
                    }

                    var converter = TypeDescriptor.GetConverter(prop.PropertyType);

                    if (converter != null)
                    {
                        var convertedValue = converter.ConvertFrom(stringData);
                        prop.SetValue(result, convertedValue);
                    }
                }
            }
        }
    }
}
