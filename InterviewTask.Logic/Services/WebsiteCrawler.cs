using InterviewTask.Logic.Crawler;
using InterviewTask.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InterviewTask.Logic.Services
{
    public class WebsiteCrawler
    {
        private readonly HtmlCrawler _parserHtml;
        private readonly SitemapCrawler _parserSitemap;
        private readonly LinkRequest _linkRequest;
        private readonly int _timeSleep;

        public WebsiteCrawler(HtmlCrawler parseHtml, SitemapCrawler parseSitemap, LinkRequest linkRequest, int timeSleep = 150)
        {
            _parserHtml = parseHtml;
            _parserSitemap = parseSitemap;
            _linkRequest = linkRequest;

            _timeSleep = timeSleep;
        }

        public IEnumerable<DataForLink> Start(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var listLinksHtml = _parserHtml.StartParse(inputLink);

            var listLinksSitemap = _parserSitemap.Parse(inputLink);

            var listAllLinks = ConcatLists(listLinksHtml, listLinksSitemap);

            TakeTimeResponseLink(listAllLinks);

            return listAllLinks;
        }

        private IEnumerable<DataForLink> ConcatLists(IEnumerable<Uri> listLinksHtml, IEnumerable<Uri> listLinksSitemap)
        {
            var intersectLinks = listLinksHtml.Intersect(listLinksSitemap);

            var listUniqueLinks = intersectLinks.Select(s => new DataForLink()
            {
                Link = s,
                IsLinkFromHtml = true,
                IsLinkFromSitemap = true
            });

            var onlyLinksHtml = listLinksHtml.Except(intersectLinks)
                .Select(s => new DataForLink()
                {
                    Link = s,
                    IsLinkFromHtml = true,
                    IsLinkFromSitemap = false
                });

            var onlyLinksSitemap = listLinksSitemap.Except(intersectLinks)
                .Select(s => new DataForLink()
                {
                    Link = s,
                    IsLinkFromHtml = false,
                    IsLinkFromSitemap = true
                });

            var listAllLinks = new List<DataForLink>(listUniqueLinks);
            listAllLinks.AddRange(onlyLinksHtml);
            listAllLinks.AddRange(onlyLinksSitemap);

            return listAllLinks;
        }

        private void TakeTimeResponseLink(IEnumerable<DataForLink> list)
        {
            foreach (var dataForLink in list)
            {
                Thread.Sleep(_timeSleep);
                dataForLink.ResponseTime = _linkRequest.LinkResponseTime(dataForLink.Link);
            }
        }
    }
}
