using InterviewTask.CrawlerLogic.Models;
using InterviewTask.EntityFramework.Entities;
using InterviewTask.Logic.Models.Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.Logic.Services
{
    public class DatabaseService
    {
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<CrawlingResult> _crawlingResultRepository;

        public DatabaseService(IRepository<Test> testRepository, IRepository<CrawlingResult> crawlingResultRepository)
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
                    return new CrawlingResult
                    {
                        Url = linksResponse.Url,
                        IsLinkFromHtml = linksFlags.IsLinkFromHtml,
                        IsLinkFromSitemap = linksFlags.IsLinkFromSitemap,
                        ResponseTime = linksResponse.ResponseTime
                    };
                }).ToList();

            var test = new Test
            {
                BaseUrl = baseLink,
                Links = listLinksForDb
            };

            await _testRepository.AddAsync(test);
            await _testRepository.SaveChangesAsync();
        }

        public async Task<ResultPagination<TestModel>> GetPageTestsAsync(int currentPage, int pageSize)
        {
            var links = _testRepository.GetAllAsNoTracking();
            var linksPart = await _testRepository.GetPageAsync(links, currentPage, pageSize);

            var listTestModels = linksPart.Result.Select(s => new TestModel { Id = s.Id, BaseUrl = s.BaseUrl, ParsingDate = s.ParsingDate });

            var totalPage = (int)Math.Ceiling(linksPart.TotalCount / (decimal)pageSize);

            var resultPagination = new ResultPagination<TestModel>
            {
                List = listTestModels,
                TotalCount = linksPart.TotalCount,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPage = totalPage
            };

            return resultPagination;
        }

        public IEnumerable<Result> GetListAllLinks(int id)
        {
            return _crawlingResultRepository.GetAllAsNoTracking()
                                            .Where(s => s.TestId == id)
                                            .Select(s => new Result
                                            {
                                                Url = s.Url,
                                                ResponseTime = s.ResponseTime,
                                                IsLinkFromHtml = s.IsLinkFromHtml,
                                                IsLinkFromSitemap = s.IsLinkFromSitemap
                                            })
                                            .ToList();
        }
    }
}
