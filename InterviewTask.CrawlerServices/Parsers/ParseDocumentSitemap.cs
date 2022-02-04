using InterviewTask.CrawlerLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace InterviewTask.CrawlerLogic.Parsers
{
    public class ParseDocumentSitemap
    {
        private readonly Converter _converter;

        public ParseDocumentSitemap(Converter converter)
        {
            _converter = converter;
        }

        public virtual IEnumerable<Uri> ParseDocument(string document, Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            if (string.IsNullOrEmpty(document))
            {
                return Enumerable.Empty<Uri>();
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(document);

            string tagForClipping = "loc";

            var xmlListLinks = xmlDocument.GetElementsByTagName(tagForClipping);

            if (xmlListLinks == null)
            {
                return Enumerable.Empty<Uri>();
            }

            var listLinksSitemap = new List<Uri>(xmlListLinks.Count);

            var listLinks = xmlListLinks.Cast<XmlNode>()
                                        .Select(s => _converter.ToUri(s.InnerText, baseLink))
                                        .Where(s => s != null);

            foreach (var item in listLinks)
            {
                if (!listLinksSitemap.Contains(item))
                {
                    listLinksSitemap.Add(item);
                }
            }

            return listLinksSitemap;
        }
    }
}
