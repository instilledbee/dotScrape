using DotScrape.Attributes;
using DotScrape.Attributes.Selectors;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace DotScrape.HtmlAgilityPack
{
    public class HtmlAgilityScraper : IScraper
    {
        private readonly HtmlDocument _htmlDocument;

        public HtmlAgilityScraper(HtmlDocument htmlDocument)
        {
            _htmlDocument = htmlDocument;
        }

        public TModel Scrape<TModel>() where TModel: new()
        {
            var result = new TModel();

            MapPropertiesWithCssSelectors(result, _htmlDocument.DocumentNode);
            MapPropertiesWithXPathSelectors(result, _htmlDocument.DocumentNode);

            return result;
        }

        private void MapPropertiesWithXPathSelectors<TModel>(TModel result, HtmlNode rootNode)
        {
            var mappableXPathProps = result.GetType()
                                         .GetProperties()
                                         .Where(p => Attribute.IsDefined(p, typeof(ViaXPathAttribute)));

            foreach (var prop in mappableXPathProps)
            {
                var attr = prop.GetCustomAttribute<ViaXPathAttribute>();
                var selector = attr.Xpath;

                var matchingNodes = rootNode.SelectNodes(selector);

                if (prop.PropertyType.GetInterface(nameof(ICollection)) != null)
                {
                    MapCollectionProperty(result, prop, matchingNodes);
                }
                else
                {
                    var node = matchingNodes.FirstOrDefault();
                    MapProperty(result, prop, node.ToScrapeHtmlNode());
                }
            }
        }

        private void MapPropertiesWithCssSelectors<TModel>(TModel result, HtmlNode rootNode)
        {
            var mappableCssProps = result.GetType()
                                         .GetProperties()
                                         .Where(p => Attribute.IsDefined(p, typeof(ViaCssAttribute)));

            foreach (var prop in mappableCssProps)
            {
                var attr = prop.GetCustomAttribute<ViaCssAttribute>();
                var selector = attr.CssSelector;

                var matchingNodes = rootNode.QuerySelectorAll(selector);

                if (matchingNodes != null)
                {
                    if (prop.PropertyType.GetInterface(nameof(ICollection)) != null)
                    {
                        MapCollectionProperty(result, prop, matchingNodes);
                    }
                    else
                    {
                        var node = matchingNodes.FirstOrDefault();
                        MapProperty(result, prop, node.ToScrapeHtmlNode());
                    }
                }
            }
        }

        private void MapCollectionProperty<TModel>(TModel result, PropertyInfo collectionProp, IEnumerable<HtmlNode> matchingNodes)
        {
            var collectionType = collectionProp.PropertyType;
            var collection = Activator.CreateInstance(collectionType);
            var genericType = collectionType.GetGenericArguments()[0];

            foreach (var node in matchingNodes)
            {
                var mappedItem = Activator.CreateInstance(genericType);

                MapPropertiesWithCssSelectors(mappedItem, node);
                MapPropertiesWithXPathSelectors(mappedItem, node);

                collectionType.GetMethod("Add").Invoke(collection, new[] { mappedItem });
            }

            collectionProp.SetValue(result, collection);
        }

        private void MapProperty<TModel>(TModel result, PropertyInfo prop, ScrapeHtmlNode node)
        {
            var stringData = node?.InnerText;

            var transformAttributes = prop.GetCustomAttributes()
                                          .Where(a => a.GetType().IsSubclassOf(typeof(TransformAttribute)));

            foreach (TransformAttribute attr in transformAttributes)
                stringData = attr.Visit(stringData, node);

            if (prop.PropertyType == typeof(string))
            {
                prop.SetValue(result, stringData);
                return;
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
