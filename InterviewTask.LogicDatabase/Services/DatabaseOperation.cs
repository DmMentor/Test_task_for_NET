using InterviewTask.EntityFramework.Entities;
using InterviewTask.LogicCrawler.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InterviewTask.LogicDatabase.Services
{
    public class DatabaseOperation
    {
        private readonly IRepository<Test> _testRepository;

        public DatabaseOperation(IRepository<Test> testRepository)
        {
            _testRepository = testRepository;
        }

        public void SaveToDatabase(Uri baseLink, IEnumerable<Link> listLinksFlags, IEnumerable<LinkWithResponse> listLinksWithResponse)
        {
            var listLinksForDb = listLinksFlags.Join(
                listLinksWithResponse,
                linksFlags => linksFlags.Url,
                linksResponse => linksResponse.Url,
                (linksFlags, linksResponse) =>
                {
                    return new CrawlingResult()
                    {
                        Url = linksResponse.Url,
                        IsLinkFromHtml = linksFlags.IsLinkFromHtml,
                        IsLinkFromSitemap = linksFlags.IsLinkFromSitemap,
                        ResponseTime = linksResponse.ResponseTime
                    };
                }).ToList();

            var test = new Test() { BaseUrl = baseLink, Links = listLinksForDb };

            _testRepository.Add(test);
            _testRepository.SaveChanges();
        }
    }
}
