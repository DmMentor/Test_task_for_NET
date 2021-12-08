using System;
using System.Collections.Generic;
using System.Xml;

namespace InterviewTask.Logic.Parser
{
    public class ParseDocumentSitemap
    {
        public IEnumerable<Uri> ParseDocument(string document)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(document);

            string tagForClipping = "loc";

            var xmlListLinks = xmlDocument.GetElementsByTagName(tagForClipping);

            if (xmlListLinks == null)
            {
                return null;
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
