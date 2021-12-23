using InterviewTask.LogicCrawler.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.ConsoleApp
{
    public class LinksDisplay
    {
        public void DisplayHtmlLinks(IEnumerable<Link> listAllLinks)
        {
            int count = 1;

            Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site:");

            foreach (var info in listAllLinks.Where(i => !i.IsLinkFromHtml && i.IsLinkFromSitemap))
            {
                Console.WriteLine($"{count++})Url: {info.Url}");
            }
        }

        public void DisplaySitemapLinks(IEnumerable<Link> listAllLinks)
        {
            int count = 1;

            Console.WriteLine("\nUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml:");
            foreach (var info in listAllLinks.Where(i => i.IsLinkFromHtml && !i.IsLinkFromSitemap))
            {
                Console.WriteLine($"{count++})Url: {info.Url}");
            }
        }

        public void DisplayAllLinks(IEnumerable<LinkWithResponse> listAllLinks)
        {
            int count = 1;

            Console.WriteLine("\nTiming website(ms):");
            foreach (var info in listAllLinks.OrderBy(l => l.ResponseTime))
            {
                Console.WriteLine($"{count++})Url: {info.Url}\n Time response: {info.ResponseTime}ms");
            }
        }

        public void DisplayAmountLinks(IEnumerable<Link> listAllLinks)
        {
            int urlHtml = listAllLinks?.Where(l => (l.IsLinkFromHtml)).Count() ?? 0;
            int urlSitemap = listAllLinks?.Where(l => (l.IsLinkFromSitemap)).Count() ?? 0;

            Console.WriteLine($"\nUrls(html documents) found after crawling a website: {urlHtml}");
            Console.WriteLine($"Urls found in sitemap: {urlSitemap}");
        }
    }
}
