using System;
using System.Collections.Generic;
using System.Linq;

namespace Parse_site
{
    class Crawler
    {
        private readonly ParseHtml _parserHtml;
        private readonly ParseSitemap _parserSitemap;
        private readonly LinkRequest _linkRequest;

        public Crawler(ParseHtml parseHtml, ParseSitemap parseSitemap, LinkRequest linkRequest)
        {
            _parserHtml = parseHtml;
            _parserSitemap = parseSitemap;
            _linkRequest = linkRequest;
        }

        public void Start()
        {
            List<Uri> listLinksSitemap;
            List<Uri> listLinksHtml;

            listLinksHtml = _parserHtml.StartParse();

            listLinksSitemap = _parserSitemap.StartParse();

            var listAllLinks = ListCombiningLinksAndResponses(listLinksHtml, listLinksSitemap);

            int countLinksHtml = listLinksHtml?.Count ?? 0;
            int countLinksXml = listLinksSitemap?.Count ?? 0;

            DisplayResults(listAllLinks, listLinksHtml, listLinksSitemap);

            Console.WriteLine("\n\nUrls(html documents) found after crawling a website: {0}", countLinksHtml);
            Console.WriteLine("Urls found in sitemap: {0}", countLinksXml);
        }

        private List<Info> ListCombiningLinksAndResponses(List<Uri> listLinksHtml, List<Uri> listLinksSitemap)
        {
            var combinedLists = new List<Uri>(listLinksHtml);

            if (listLinksSitemap != null)
            {
                combinedLists.AddRange(listLinksSitemap);
            }

            var listAllLinks = new List<Info>();

            foreach (var link in combinedLists.Distinct())
            {
                int response = _linkRequest.SendRequest(link);

                Info info = new Info(link, response);

                listAllLinks.Add(info);
            }

            return listAllLinks;
        }


        private void DisplayList(IEnumerable<Uri> list)
        {
            int count = 1;

            foreach (var link in list)
            {
                Console.WriteLine("{0})Url: {1}\n", count++, link);
            }

            Console.WriteLine("\n\n\n");
        }

        private void DisplayResults(List<Info> listAllLinks, List<Uri> listLinksHtml, List<Uri> listLinksSitemap)
        {
            int count = 1;
            var intersectLinks = listLinksHtml.Intersect(listLinksSitemap);

            if (listLinksSitemap != null)
            {
                Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site:");
                DisplayList(listLinksSitemap.Except(intersectLinks));
            }

            if (listLinksHtml != null)
            {
                Console.WriteLine("Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml:");
                DisplayList(listLinksHtml.Except(intersectLinks));
            }

            listAllLinks.Sort();

            Console.WriteLine("Timing website(ms):");
            foreach (var info in listAllLinks)
            {
                if (info != null)
                {
                    Console.WriteLine("{0})Url: {1}\n Time response: {2}ms\n", count++, info.uri.AbsoluteUri, info.response);
                }
            }
        }
    }
}
