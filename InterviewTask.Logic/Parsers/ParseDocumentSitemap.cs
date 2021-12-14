using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace InterviewTask.Logic.Parsers
{
    public class ParseDocumentSitemap
    {
        public virtual IEnumerable<Uri> ParseDocument(string document)
        {
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

            ICollection<Uri> listLinksSitemap = new List<Uri>(xmlListLinks.Count);

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
