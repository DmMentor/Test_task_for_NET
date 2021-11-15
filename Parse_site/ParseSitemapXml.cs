using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Xml;

namespace Parse_site
{
    class ParseSitemapXml : IParse
    {
        private Uri _uri;
        public ParseSitemapXml(Uri uri)
        {
            _uri = uri;
        }

        public async Task<List<string>> ParseAsync()
        {
            string sitemapXml;

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            try
            {
                sitemapXml = await client.DownloadStringTaskAsync(Uri.UriSchemeHttps + "://" + _uri.Host + "/sitemap.xml");
            }
            catch (WebException)
            {
                return null;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sitemapXml);

            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("loc");

            if (xmlNodeList.Count == 0)
                return null;

            var listUrlsSitemap = new List<string>(xmlNodeList.Count);

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (!listUrlsSitemap.Contains(xmlNode.InnerText))
                {
                    listUrlsSitemap.Add(xmlNode.InnerText);
                }
            }

            return listUrlsSitemap;
        }
    }
}
