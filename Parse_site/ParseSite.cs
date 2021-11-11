using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;

namespace Parse_site
{
    class ParseSite
    {
        Uri _urlParse;

        List<(string url, double response)> listUrlsSitemap = null;
        List<(string url, double response)> listUrlsHtml = null;

        public Uri urlParse
        {
            get => _urlParse;
            set => _urlParse = value;
        }

        public ParseSite(string url)
        {
            _urlParse = new Uri(url);
        }

        public async Task Start()
        {
            try
            {
                await HtmlDocument();
            }
            catch (WebException)
            {
                Console.WriteLine("Unable to retrieve html document");
                return;
            }

            await SitemapXml();

            int i = 1;

            ComparerListUrls comparerList = new ComparerListUrls();

            if (listUrlsSitemap != null && listUrlsSitemap.Count != 0)
            {
                List<(string url, double response)> list_html = new List<(string url, double response)>(listUrlsHtml);

                foreach (var tuplesHtml in listUrlsHtml)
                {
                    foreach (var tuplesXml in listUrlsSitemap)
                    {
                        if (tuplesHtml.url == tuplesXml.url)
                        {
                            listUrlsHtml.Remove(tuplesHtml);
                            listUrlsSitemap.Remove(tuplesXml);
                            break;
                        }
                    }
                }


                listUrlsSitemap.Sort(comparerList);

                Console.WriteLine("Sitemap xml:");
                foreach (var tuple in listUrlsSitemap)
                {
                    Console.WriteLine("{0})Url:{1}\n {2}ms\n", i++, tuple.url, tuple.response);
                }

                Console.WriteLine("\n\n\n");
            }


            if (listUrlsHtml != null && listUrlsHtml.Count != 0)
            {
                Console.WriteLine("Html document:");

                listUrlsHtml.Sort(comparerList);

                i = 1;
                foreach (var tuple in listUrlsHtml)
                {
                    Console.WriteLine("{0})Url:{1}\n {2}ms\n", i++, tuple.url, tuple.response);
                }

                Console.WriteLine("\n\nUrls(html documents) found after crawling a website: {0}", listUrlsHtml.Count);
            }
            else
                Console.WriteLine("\n\nUrls(html documents) found after crawling a website: 0");


            if (listUrlsSitemap != null)
                Console.WriteLine("Urls found in sitemap: {0}", listUrlsSitemap.Count);
            else
                Console.WriteLine("Urls found in sitemap: 0");
        }

        #region MethodsParse

        private async Task HtmlDocument()
        {
            string htmlDocument;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_urlParse);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    htmlDocument = await streamReader.ReadToEndAsync();
                }
            }

            Regex reg = new Regex("<a href=\"(.*?)\"", RegexOptions.Singleline);
            MatchCollection matchCollectionUrls = reg.Matches(htmlDocument);

            listUrlsHtml = new List<(string url, double response)>(matchCollectionUrls.Count);

            Stopwatch time = new Stopwatch();

            foreach (Match matchUrl in matchCollectionUrls)
            {
                string url;
                if (!matchUrl.Groups[1].Value.Contains("https://"))
                    url = Uri.UriSchemeHttps + "://" + _urlParse.Host + matchUrl.Groups[1].Value;
                else
                    url = matchUrl.Groups[1].Value;

                try
                {
                    time.Start();
                    ((HttpWebRequest)WebRequest.Create(url)).GetResponse().Close();
                    time.Stop();

                    listUrlsHtml.Add((url, time.Elapsed.TotalMilliseconds));
                }
                catch (WebException)
                {
                    url += " --- not work";
                    listUrlsHtml.Add((url, -1));
                }
                finally
                {
                    time.Reset();
                }
            }
        }

        private async Task SitemapXml()
        {
            string sitemapXml;

            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            try
            {
                sitemapXml = await client.DownloadStringTaskAsync(Uri.UriSchemeHttps + "://" + _urlParse.Host + "/sitemap.xml");
            }
            catch (WebException)
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(sitemapXml);

            XmlNodeList xmlNodeList = xmlDocument.GetElementsByTagName("loc");

            listUrlsSitemap = new List<(string url, double response)>(xmlNodeList.Count);

            Stopwatch time = new Stopwatch();

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                time.Start();
                ((HttpWebRequest)WebRequest.Create(xmlNode.InnerText)).GetResponse().Close();
                time.Stop();

                try
                {
                    listUrlsSitemap.Add((xmlNode.InnerText, time.Elapsed.TotalMilliseconds));
                }
                catch (WebException)
                {
                    listUrlsSitemap.Add((xmlNode.InnerText + " --- not work", -1));
                }
                finally
                {
                    time.Reset();
                }
            }
        }

        #endregion
    }
}
