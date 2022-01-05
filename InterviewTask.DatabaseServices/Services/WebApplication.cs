using InterviewTask.CrawlerLogic.Services;
using System;
using System.Threading.Tasks;

namespace InterviewTask.Logic.Services
{
    public class WebApplication
    {
        private readonly WebsiteCrawler _crawler;
        private readonly LinkRequest _linkRequest;
        private readonly DatabaseOperation _databaseOperation;

        public WebApplication(WebsiteCrawler crawler, LinkRequest linkRequest, DatabaseOperation databaseOperation)
        {
            _crawler = crawler;
            _linkRequest = linkRequest;
            _databaseOperation = databaseOperation;
        }

        public async Task StartAsync(Uri link)
        {
            var listAllLinks = await _crawler.StartAsync(link);
            var listAllLinksWithResponse = await _linkRequest.GetListWithLinksResponseTimeAsync(listAllLinks);

            await _databaseOperation.SaveToDatabaseAsync(link, listAllLinks, listAllLinksWithResponse);
        }
    }
}
