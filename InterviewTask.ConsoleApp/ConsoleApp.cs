using InterviewTask.Logic.Models;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;

namespace InterviewTask.ConsoleApp
{
    internal class ConsoleApp
    {
        private readonly LinksDisplay _linksDisplay;
        private readonly LinkRequest _linkRequest;
        private readonly WebsiteCrawler _crawler;

        public ConsoleApp(LinksDisplay linksDisplay, LinkRequest linkRequest, WebsiteCrawler crawler)
        {
            _linksDisplay = linksDisplay;
            _linkRequest = linkRequest;
            _crawler = crawler;
        }

        public void Run()
        {
            Uri inputLink = InputLink();

            Console.WriteLine($"Starting parsing website....{Environment.NewLine}");

            var listAllLinks = _crawler.Start(inputLink);
            var listAllLinksWithResponse = _linkRequest.GetListWithLinksResponseTime(listAllLinks);

            Display(listAllLinks, listAllLinksWithResponse);
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
