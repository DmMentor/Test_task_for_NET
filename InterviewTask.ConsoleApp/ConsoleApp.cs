using InterviewTask.Logic.Crawlers;
using InterviewTask.Logic.Models;
using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InterviewTask.ConsoleApp
{
    internal class ConsoleApp
    {
        private readonly LinksDisplay _linksDisplay;
        private readonly Converter _converter;
        private readonly LinkHandling _linkHandling;

        public ConsoleApp(LinksDisplay linksDisplay, Converter converter, LinkHandling linkHandling)
        {
            _linksDisplay = linksDisplay;
            _converter = converter;
            _linkHandling = linkHandling;
        }

        private WebsiteCrawler SetupCrawler()
        {
            var parseDocumentHtml = new ParseDocumentHtml();
            var parseDocumentXml = new ParseDocumentSitemap();
            var downloadDocument = new LinkHandling();

            var parseHtml = new HtmlCrawler(parseDocumentHtml, downloadDocument, _converter);
            var parseSitemap = new SitemapCrawler(parseDocumentXml, downloadDocument);

            return new WebsiteCrawler(parseHtml, parseSitemap);
        }

        public void Run()
        {
            try
            {
                Uri inputLink = InputLink();

                if (inputLink == null)
                {
                    return;
                }

                Console.WriteLine($"Starting parsing website....{Environment.NewLine}");

                WebsiteCrawler crawler = SetupCrawler();
                var listAllLinks = crawler.Start(inputLink);

                _linksDisplay.DisplayHtmlLinks(listAllLinks);
                _linksDisplay.DisplaySitemapLinks(listAllLinks);

                var listAllLinksWithResponse = _converter.ToLinkWithResponse(listAllLinks);
                GetResponseLinks(listAllLinksWithResponse);

                _linksDisplay.DisplayAllLinks(listAllLinksWithResponse);

                _linksDisplay.DisplayAmountLinks(listAllLinks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Uri InputLink()
        {
            Console.Write("Input url website: ");
            string link = Console.ReadLine();

            if (Uri.TryCreate(link, UriKind.Absolute, out Uri linkUri))
            {
                return linkUri;
            }

            Console.WriteLine("Link is invalid");
            return null;
        }

        private void GetResponseLinks(IEnumerable<LinkWithResponse> inputListLinks)
        {
            foreach (var link in inputListLinks)
            {
                var timer = Stopwatch.StartNew();
                _linkHandling.GetLinkResponse(link.Url);
                timer.Stop();

                link.ResponseTime = timer.Elapsed.Milliseconds;
            }
        }
    }
}
