using InterviewTask.CrawlerServices.Crawlers;
using InterviewTask.CrawlerServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.CrawlerServices.Services
{
    public class WebsiteCrawler
    {
        private readonly HtmlCrawler _parserHtml;
        private readonly SitemapCrawler _parserSitemap;

        public WebsiteCrawler(HtmlCrawler parseHtml, SitemapCrawler parseSitemap)
        {
            _parserHtml = parseHtml;
            _parserSitemap = parseSitemap;
        }

        public IEnumerable<Link> Start(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var listLinksHtml = _parserHtml.StartParse(inputLink);

            var listLinksSitemap = _parserSitemap.Parse(inputLink);

            var listAllLinks = ConcatLists(listLinksHtml, listLinksSitemap);

            return listAllLinks.ToList();
        }

        private IEnumerable<Link> ConcatLists(IEnumerable<Uri> listLinksHtml, IEnumerable<Uri> listLinksSitemap)
        {
            var intersectLinks = listLinksHtml.Intersect(listLinksSitemap);

            var listUniqueLinks = intersectLinks.Select(s => new Link()
            {
                Url = s,
                IsLinkFromHtml = true,
                IsLinkFromSitemap = true
            });

            var onlyLinksHtml = listLinksHtml.Except(intersectLinks)
                .Select(s => new Link()
                {
                    Url = s,
                    IsLinkFromHtml = true,
                    IsLinkFromSitemap = false
                });

            var onlyLinksSitemap = listLinksSitemap.Except(intersectLinks)
                .Select(s => new Link()
                {
                    Url = s,
                    IsLinkFromHtml = false,
                    IsLinkFromSitemap = true
                });

            var listAllLinks = new List<Link>(listUniqueLinks);
            listAllLinks.AddRange(onlyLinksHtml);
            listAllLinks.AddRange(onlyLinksSitemap);

            return listAllLinks;
        }
    }
}
