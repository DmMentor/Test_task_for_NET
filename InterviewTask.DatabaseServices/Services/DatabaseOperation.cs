using InterviewTask.EntityFramework.Entities;
using InterviewTask.CrawlerLogic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using InterviewTask.Logic.Models;

namespace InterviewTask.Logic.Services
{
    public class DatabaseOperation
    {
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<CrawlingResult> _crawlingResultRepository;

        public DatabaseOperation(IRepository<Test> testRepository, IRepository<CrawlingResult> crawlingResultRepository)
        {
            _testRepository = testRepository;
            _crawlingResultRepository = crawlingResultRepository;
        }

        public async Task SaveToDatabaseAsync(Uri baseLink, IEnumerable<Link> listLinksFlags, IEnumerable<LinkWithResponse> listLinksWithResponse)
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

            await _testRepository.AddAsync(test);
            await _testRepository.SaveChangesAsync();
        }

        public async Task<LinkTest> GetListTestsAsync(int currentPage, int pageSize)
        {
            var links = _testRepository.GetAllAsNoTracking();

            var linksPart = await _testRepository.GetPageAsync(links, currentPage, pageSize);

            return new LinkTest { List = linksPart.Result, TotalCount=linksPart.TotalCount };
        }

        public IEnumerable<CrawlingResult> GetListAllLinks(int id)
        {
            return _crawlingResultRepository.GetAllAsNoTracking()
                                            .Where(s => s.TestId == id)
                                            .ToList();
        }
    }
}
