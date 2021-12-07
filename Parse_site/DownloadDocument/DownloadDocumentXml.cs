using System;
using System.Net;
using System.Xml;

namespace Parse_site.Download
{
    class DownloadDocumentXml : IDownloadDocument<XmlNodeList>
    {
        public XmlNodeList DownloadDocument(string inputLink)
        {
            WebClient webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;
            string sitemapXml;

            Uri link = new Uri(inputLink);

            try
            {
                sitemapXml = webClient.DownloadString(link.Scheme + "://" + link.Host + "/sitemap.xml");
            }
            catch (WebException)
            {
                return null;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sitemapXml);

            string tagForClipping = "loc";

            XmlNodeList xmlListLink = xmlDocument.GetElementsByTagName(tagForClipping);

            return xmlListLink.Count == 0 ? null : xmlListLink;
        }
    }
}
