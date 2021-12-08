using System.Collections.Generic;
using System.Xml;
using System;

namespace ParseDocument
{
    public class ParseDocumentXml : IParseDocument
    {
        public List<Uri> ParseDocument(string document)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(document);

            string tagForClipping = "loc";

            var xmlListLinks = xmlDocument.GetElementsByTagName(tagForClipping);

            if (xmlListLinks == null)
            {
                return null;
            }

            var listLinksSitemap = new List<Uri>(xmlListLinks.Count);

            foreach (XmlNode xmlLink in xmlListLinks)
            {
                var link = new Uri(xmlLink.InnerText);

                if (!listLinksSitemap.Contains(link))
                {
                    listLinksSitemap.Add(link);
                }
            }

            return listLinksSitemap;
        }
    }
}
