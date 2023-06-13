using DotScrape.Attributes.Extractors;
using DotScrape.Attributes.Formatters;
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

        public Uri BaseUrl { get; }

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

            if (mappableXPathProps.Any())
            {
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
                        MapProperty(result, prop, node);
                    }
                }
            }
        }

        private void MapPropertiesWithCssSelectors<TModel>(TModel result, HtmlNode rootNode)
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
                            MapProperty(result, prop, node);
                        }
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

        private void MapProperty<TModel>(TModel result, PropertyInfo prop, HtmlNode node)
        {
            var stringData = node?.InnerText;

            var fromAttributeAttribute = prop.GetCustomAttribute<FromAttributeAttribute>();

            // TODO: Use visitor pattern for attribute logic
            if (fromAttributeAttribute != null)
            {
                var elementAttribute = node.GetAttributes().FirstOrDefault(a => a.Name == fromAttributeAttribute.AttributeName);

                if (elementAttribute != null)
                    stringData = elementAttribute.Value;
            }

            var trimStringAttribute = prop.GetCustomAttribute<TrimStringAttribute>();
            if (trimStringAttribute != null)
            {
                stringData = stringData?.Trim();
            }

            var substringAttribute = prop.GetCustomAttribute<SubstringAttribute>();

            if (substringAttribute != null)
            {
                var startIndex = substringAttribute.StartIndex >= 0 ? substringAttribute.StartIndex : 0;
                var length = substringAttribute.Length >= 0 ? substringAttribute.Length : ((stringData?.Length ?? 0) - startIndex);

                stringData = stringData.Substring(startIndex, length);
            }

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
