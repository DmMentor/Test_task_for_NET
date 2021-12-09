using InterviewTask.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.ConsoleApp
{
    class LinksDisplay
    {
        public void DisplayResults(IEnumerable<DataForLink> listAllLinks)
        {
            int count = 1;

            Console.WriteLine("Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site:");

            foreach (var info in listAllLinks)
            {
                if (info.IsLinkFromHtml == false && info.IsLinkFromSitemap == true)
                {
                    Console.WriteLine($"{count++})Url: {info.Link}");
                }
            }
            count = 1;

            Console.WriteLine("\nUrls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml:");
            foreach (var info in listAllLinks)
            {
                if (info.IsLinkFromHtml == true && info.IsLinkFromSitemap == false)
                {
                    Console.WriteLine($"{count++})Url: {info.Link}");
                }
            }
            count = 1;

            Console.WriteLine("\nTiming website(ms):");
            foreach (var info in listAllLinks.OrderBy(l => l.ResponseTime))
            {
                Console.WriteLine($"{count++})Url: {info.Link}\n Time response: {info.ResponseTime}ms");
            }

            int urlHtml = listAllLinks?.Where(l => (l.IsLinkFromSitemap && l.IsLinkFromHtml == true) || (l.IsLinkFromSitemap == false && l.IsLinkFromHtml == true)).Count() ?? 0;
            int urlSitemap = listAllLinks?.Where(l => (l.IsLinkFromSitemap && l.IsLinkFromHtml == true) || (l.IsLinkFromSitemap == true && l.IsLinkFromHtml == false)).Count() ?? 0;

            Console.WriteLine($"\nUrls(html documents) found after crawling a website: {urlHtml}");
            Console.WriteLine($"Urls found in sitemap: {urlSitemap}");
        }
    }
}
