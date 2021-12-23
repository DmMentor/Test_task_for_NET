using InterviewTask.CrawlerServices.Models;
using InterviewTask.CrawlerServices.Services;
using InterviewTask.DatabaseServices.Services;
using System;
using System.Collections.Generic;

namespace InterviewTask.ConsoleApp
{
    internal class ConsoleApp
    {
        private readonly LinksDisplay _linksDisplay;
        private readonly LinkRequest _linkRequest;
        private readonly WebsiteCrawler _crawler;
        private readonly DatabaseOperation _databaseOperation;

        public ConsoleApp(LinksDisplay linksDisplay, LinkRequest linkRequest, WebsiteCrawler crawler, DatabaseOperation databaseOperation)
        {
            _linksDisplay = linksDisplay;
            _linkRequest = linkRequest;
            _crawler = crawler;
            _databaseOperation = databaseOperation;
        }

        public void Run()
        {
            Uri inputLink = InputLink();

            Console.WriteLine($"Starting parsing website....{Environment.NewLine}");

            var listAllLinks = _crawler.Start(inputLink);
            var listAllLinksWithResponse = _linkRequest.GetListWithLinksResponseTime(listAllLinks);

            Display(listAllLinks, listAllLinksWithResponse);

            _databaseOperation.SaveToDatabase(inputLink, listAllLinks, listAllLinksWithResponse);
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
