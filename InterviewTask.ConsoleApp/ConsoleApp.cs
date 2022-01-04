using InterviewTask.CrawlerLogic.Models;
using InterviewTask.CrawlerLogic.Services;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task RunAsync()
        {
            Uri inputLink = InputLink();

            Console.WriteLine($"Starting parsing website....{Environment.NewLine}");

            var listAllLinks = await _crawler.StartAsync(inputLink);
            var listAllLinksWithResponse = await _linkRequest.GetListWithLinksResponseTimeAsync(listAllLinks);

            Display(listAllLinks, listAllLinksWithResponse);

            await _databaseOperation.SaveToDatabaseAsync(inputLink, listAllLinks, listAllLinksWithResponse);
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
