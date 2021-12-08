using InterviewTask.Logic.Models;
using InterviewTask.Logic.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace InterviewTask.Logic.Crawler
{
    public class WebsiteCrawler
    {
        private readonly HtmlCrawler _parserHtml;
        private readonly SitemapCrawler _parserSitemap;
        private readonly DownloadDocument _downloadDocument;
        private readonly int _timeSleep;

        public WebsiteCrawler(HtmlCrawler parseHtml, SitemapCrawler parseSitemap, DownloadDocument downloadDocument, int timeSleep = 150)
        {
            _parserHtml = parseHtml;
            _parserSitemap = parseSitemap;
            _downloadDocument = downloadDocument;

            _timeSleep = timeSleep;
        }

        public IEnumerable<Info> Start(Uri inputLink)
        {
            var listLinksHtml = _parserHtml.StartParse(inputLink);

            var listLinksSitemap = _parserSitemap.StartParse(inputLink);

            var listAllLinks = ConcatLists(listLinksHtml, listLinksSitemap);

            TakeTimeResponseLink(listAllLinks);

            return listAllLinks;
        }

        private IEnumerable<Info> ConcatLists(IEnumerable<Uri> listLinksHtml, IEnumerable<Uri> listLinksSitemap)
        {
            var intersectLinks = listLinksHtml.Intersect(listLinksSitemap);

            var listUniqueLinks = intersectLinks.Select(s => new Info()
            {
                Link = s,
                IsLinkFromHtml = true,
                IsLinkFromSitemap = true
            });

            var onlyLinksHtml = listLinksHtml.Except(intersectLinks)
                .Select(s => new Info()
                {
                    Link = s,
                    IsLinkFromHtml = true,
                    IsLinkFromSitemap = false
                });

            var onlyLinksSitemap = listLinksSitemap.Except(intersectLinks)
                .Select(s => new Info()
                {
                    Link = s,
                    IsLinkFromHtml = false,
                    IsLinkFromSitemap = true
                });

            var listAllLinks = new List<Info>(listUniqueLinks);
            listAllLinks.AddRange(onlyLinksHtml);
            listAllLinks.AddRange(onlyLinksSitemap);

            return listAllLinks;
        }

        private void TakeTimeResponseLink(IEnumerable<Info> list)
        {
            foreach (var info in list)
            {
                Thread.Sleep(_timeSleep);

                var time = Stopwatch.StartNew();

                _downloadDocument.Download(info.Link);

                time.Stop();

                info.ResponseTime = time.Elapsed.Milliseconds;
            }
        }
    }
}
