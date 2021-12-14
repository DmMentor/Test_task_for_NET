using InterviewTask.Logic.Crawlers;
using InterviewTask.Logic.Models;
using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;

namespace InterviewTask.ConsoleApp
{
    internal class ConsoleApp
    {
        private readonly LinksDisplay _linksDisplay;
        private readonly Converter _converter;
        private readonly LinkRequest _linkRequest;

        public ConsoleApp(LinksDisplay linksDisplay, Converter converter, LinkRequest linkRequest)
        {
            _linksDisplay = linksDisplay;
            _converter = converter;
            _linkRequest = linkRequest;
        }

        public void Run()
        {
            try
            {
                Uri inputLink = InputLink();

                Console.WriteLine($"Starting parsing website....{Environment.NewLine}");

                WebsiteCrawler crawler = SetupCrawler();

                var listAllLinks = crawler.Start(inputLink);
                var listAllLinksWithResponse = _linkRequest.GetListWithLinksResponseTime(listAllLinks);

                Display(listAllLinks, listAllLinksWithResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private WebsiteCrawler SetupCrawler()
        {
            var parseDocumentHtml = new ParseDocumentHtml();
            var parseDocumentXml = new ParseDocumentSitemap();

            var httpService = new HttpService();
            var downloadDocument = new LinkHandling(httpService);

            var parseHtml = new HtmlCrawler(parseDocumentHtml, downloadDocument, _converter);
            var parseSitemap = new SitemapCrawler(parseDocumentXml, downloadDocument);

            return new WebsiteCrawler(parseHtml, parseSitemap);
        }

        private Uri InputLink()
        {
            Console.Write("Input url website: ");
            string link = Console.ReadLine();

            if (Uri.TryCreate(link, UriKind.Absolute, out Uri linkUri))
            {
                return linkUri;
            }

            throw new UriFormatException("Link is invalid");
        }

        private void Display(IEnumerable<Link> listAllLinks, IEnumerable<LinkWithResponse> listAllLinksWithResponse)
        {
            _linksDisplay.DisplayHtmlLinks(listAllLinks);
            _linksDisplay.DisplaySitemapLinks(listAllLinks);

            _linksDisplay.DisplayAllLinks(listAllLinksWithResponse);
            _linksDisplay.DisplayAmountLinks(listAllLinks);
        }
    }
}
