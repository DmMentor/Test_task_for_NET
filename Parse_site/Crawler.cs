using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Linq;
using ParseDocument;

namespace Parse_site
{
    class Crawler
    {
        private readonly ParseHtml _parserHtml;
        private readonly ParseSitemap _parserSitemap;

        public Crawler(string link)
        {
            var linkToParse = new Uri(link);

            _parserHtml = new ParseHtml(linkToParse, new ParseDocumentHtml(linkToParse));
            _parserSitemap = new ParseSitemap(linkToParse, new ParseDocumentXml());
        }

        public void StartAsync()
        {
            List<Uri> listLinksSitemap;
            List<Uri> listLinksHtml;

            try
            {
                listLinksHtml = _parserHtml.StartParse();
            }
            catch (WebException)
            {
                Console.WriteLine("Unable to retrieve html document");
                return;
            }

            listLinksSitemap = _parserSitemap.StartParse();

            var listAllLinks = ConcatLists(listLinksHtml, listLinksSitemap);

            int countLinksHtml = listLinksHtml?.Count ?? 0;
            int countLinksXml = listLinksSitemap?.Count ?? 0;

            DisplayResults(listAllLinks, listLinksHtml, listLinksSitemap);

            Console.WriteLine("\n\nUrls(html documents) found after crawling a website: {0}", countLinksHtml);
            Console.WriteLine("Urls found in sitemap: {0}", countLinksXml);
        }

        private List<Info> ConcatLists(List<Uri> listLinksHtml, List<Uri> listLinksSitemap)
        {
            var time = new Stopwatch();

            var combinedLists = new List<Uri>(listLinksHtml);
            combinedLists.AddRange(listLinksSitemap);

            var listAllLinks = new List<Info>();

            foreach (var link in combinedLists.Distinct())
            {
                Info info;
                try
                {
                    time.Start();
                    ((HttpWebRequest)WebRequest.Create(link)).GetResponse().Close();
                    time.Stop();

                    info = new Info(link, time.Elapsed.Milliseconds);

                    listAllLinks.Add(info);
                }
                catch (WebException)
                {
                    listAllLinks.Add(null);
                }
                finally
                {
                    time.Reset();
                }
            }

            return listAllLinks;
        }

        private void DisplayResults(List<Info> listUrls, List<Uri> listLinksHtml, List<Uri> listLinksSitemap)
        {
            int i = 1;
            var intersectLinks = listLinksHtml?.Intersect(listLinksSitemap);

            if (listLinksSitemap != null)
            {
                Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site:");
                foreach (var url in listLinksSitemap.Except(intersectLinks))
                {
                    Console.WriteLine("{0})Url: {1}\n", i++, url);
                }

                i = 1;
                Console.WriteLine("\n\n\n");
            }

            if (listLinksHtml != null)
            {
                Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml:");
                foreach (var url in listLinksHtml.Except(intersectLinks))
                {
                    Console.WriteLine("{0})Url: {1}\n", i++, url);
                }

                i = 1;
                Console.WriteLine("\n\n\n");
            }

            listUrls.Sort();

            Console.WriteLine("Timing website(ms):");
            foreach (var item in listUrls)
            {
                Console.WriteLine("{0})Url: {1}\n Time response: {2}ms\n", i++, item?._uri.AbsoluteUri ?? "error", item._response);
            }
        }
    }
}
