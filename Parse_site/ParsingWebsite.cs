using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace Parse_site
{
    class ParsingWebsite
    {
        List<(string url, double response)> listUrlsSitemap = null;
        List<(string url, double response)> listUrlsHtml = null;

        ParseHtmlDocument parserHtml;
        ParseSitemapXml parserSitemap;

        public ParsingWebsite(string url)
        {
            parserHtml = new ParseHtmlDocument(new Uri(url));
            parserSitemap = new ParseSitemapXml(new Uri(url));
        }

        public async Task StartAsync()
        {
            try
            {
                listUrlsHtml = await parserHtml.ParseAsync();
            }
            catch (WebException)
            {
                Console.WriteLine("Unable to retrieve html document");
                return;
            }

            listUrlsSitemap = await parserSitemap.ParseAsync();

            ResultsProcessing();
            ShowResults();
        }

        private void ResultsProcessing()
        {
            ComparerListUrls comparerList = new ComparerListUrls();

            if (listUrlsSitemap != null && listUrlsSitemap.Count != 0)
            {
                List<(string url, double response)> list_html = new List<(string url, double response)>(listUrlsHtml);

                foreach (var tuplesHtml in list_html)
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
            }

            if (listUrlsHtml.Count != 0)
                listUrlsHtml.Sort(comparerList);
        }

        private void ShowResults()
        {
            int i = 1;
            if (listUrlsSitemap != null && listUrlsSitemap.Count != 0)
            {
                Console.WriteLine("Sitemap xml:");
                foreach (var tuple in listUrlsSitemap)
                {
                    Console.WriteLine("{0})Url: {1}\n Time response: {2}ms\n", i++, tuple.url, tuple.response);
                }

                i = 1;
                Console.WriteLine("\n\n\n");
            }

            if (listUrlsHtml != null && listUrlsHtml.Count != 0)
            {
                Console.WriteLine("Html document:");

                foreach (var tuple in listUrlsHtml)
                {
                    Console.WriteLine("{0})Url: {1}\n Time response: {2}ms\n", i++, tuple.url, tuple.response);
                }
            }

            Console.WriteLine("\n\nUrls(html documents) found after crawling a website: {0}", listUrlsHtml?.Count ?? 0);
            Console.WriteLine("Urls found in sitemap: {0}", listUrlsSitemap?.Count ?? 0);
        }
    }
}
