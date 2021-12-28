using InterviewTask.EntityFramework.Entities;
using InterviewTask.CrawlerServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InterviewTask.DatabaseServices.Services
{
    public class DatabaseOperation
    {
        private readonly IRepository<Test> _testRepository;
        IRepository<CrawlingResult> _crawlingResultRepository;

        public DatabaseOperation(IRepository<Test> testRepository, IRepository<CrawlingResult> crawlingResultRepository)
        {
            _testRepository = testRepository;
            _crawlingResultRepository = crawlingResultRepository;
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

        public IEnumerable<Test> GetListTests()
        {
            return _testRepository.GetAllAsNoTracking();
        }

        public IQueryable<CrawlingResult> GetListLinks(int id)
        {
            return _crawlingResultRepository.GetAllAsNoTracking()
                                            .Where(s => s.TestId == id);
        }
    }
}
