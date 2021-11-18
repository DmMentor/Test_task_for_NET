using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;

namespace Parse_site
{
    class ParsingWebsite
    {
        List<string> listUrlsSitemap = null;
        List<string> listUrlsHtml = null;
        List<(string url, double response)> listUrls = null;

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

            listUrls = new List<(string url, double response)>(listUrlsSitemap?.Count ?? 0 + listUrlsHtml?.Count ?? 0);

            int urlHtml = listUrlsHtml?.Count ?? 0;
            int urlXml = listUrlsSitemap?.Count ?? 0;

            ConcatLists();
            ResultsProcessing();
            ShowResults();

            Console.WriteLine("\n\nUrls(html documents) found after crawling a website: {0}", urlHtml);
            Console.WriteLine("Urls found in sitemap: {0}", urlXml);
        }

        private void ResultsProcessing()
        {
            if (listUrlsSitemap != null)
            {
                List<string> list_html = new List<string>(listUrlsHtml);

                foreach (var urlHtml in list_html)
                {
                    foreach (var urlXml in listUrlsSitemap)
                    {
                        if (urlHtml == urlXml)
                        {
                            listUrlsHtml.Remove(urlHtml);
                            listUrlsSitemap.Remove(urlXml);
                            break;
                        }
                    }
                }
            }
        }

        private void ConcatLists()
        {
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();

            List<string> list = new List<string>();

            if (listUrlsSitemap != null)
            {
                foreach (var urlSitemap in listUrlsSitemap)
                {
                    if (!list.Contains(urlSitemap))
                    {
                        list.Add(urlSitemap);
                    }
                }
            }

            if (listUrlsHtml != null)
            {
                foreach (var urlHtml in listUrlsHtml)
                {
                    if (!list.Contains(urlHtml))
                    {
                        list.Add(urlHtml);
                    }
                }
            }

            foreach (var url in list)
            {
                try
                {
                    time.Start();
                    ((HttpWebRequest)WebRequest.Create(url)).GetResponse().Close();
                    time.Stop();

                    listUrls.Add((url, time.Elapsed.TotalMilliseconds));
                }
                catch (WebException)
                {
                    listUrls.Add((url + " --- not work", -1));
                }
                finally
                {
                    time.Reset();
                }
            }

            listUrls.Sort(new ComparerListUrls());
        }

        private void ShowResults()
        {
            int i = 1;
            if (listUrlsSitemap != null)
            {
                Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site:");
                foreach (var url in listUrlsSitemap)
                {
                    Console.WriteLine("{0})Url: {1}\n", i++, url);
                }

                i = 1;
                Console.WriteLine("\n\n\n");
            }

            if (listUrlsHtml != null)
            {
                Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml:");
                foreach (var url in listUrlsHtml)
                {
                    Console.WriteLine("{0})Url: {1}\n", i++, url);
                }

                i = 1;
                Console.WriteLine("\n\n\n");
            }

            Console.WriteLine("Timing website(ms):");
            foreach (var tuple in listUrls)
            {
                Console.WriteLine("{0})Url: {1}\n Time response: {2}ms\n", i++, tuple.url, tuple.response);
            }
        }
    }
}
