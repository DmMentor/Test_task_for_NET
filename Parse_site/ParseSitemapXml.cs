using System;
using System.Diagnostics;
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

        public async Task<List<(string, double)>> ParseAsync()
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

            var listUrlsSitemap = new List<(string url, double response)>(xmlNodeList.Count);
            var listUrls = new List<string>(xmlNodeList.Count);

            Stopwatch time = new Stopwatch();

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (!listUrls.Contains(xmlNode.InnerText))
                {
                    try
                    {
                        time.Start();
                        ((HttpWebRequest)WebRequest.Create(xmlNode.InnerText)).GetResponse().Close();
                        time.Stop();

                        listUrlsSitemap.Add((xmlNode.InnerText, time.Elapsed.TotalMilliseconds));
                        listUrls.Add(xmlNode.InnerText);
                    }
                    catch (WebException)
                    {
                        listUrlsSitemap.Add((xmlNode.InnerText + " --- not work", -1));
                        listUrls.Add(xmlNode.InnerText);
                    }
                    finally
                    {
                        time.Reset();
                    }
                }
            }

            return listUrlsSitemap;
        }
    }
}
