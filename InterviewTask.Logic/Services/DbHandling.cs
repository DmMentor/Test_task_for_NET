using InterviewTask.EntityFramework.Entities;
using InterviewTask.Logic.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace InterviewTask.Logic.Services
{
    public class DbHandling
    {
        private readonly IRepository<CrawlingResult> _linkRepository;
        private readonly IRepository<Test> _testRepository;

        public DbHandling(IRepository<CrawlingResult> repositoryLink, IRepository<Test> repositoryTest)
        {
            _linkRepository = repositoryLink;
            _testRepository = repositoryTest;
        }

        public void SaveDatabase(Uri baseLink, IEnumerable<Link> listLinksFlags, IEnumerable<LinkWithResponse> listLinksWithResponse)
        {

            var listLinksForDb = listLinksFlags.Join(listLinksWithResponse,
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

            var test = new Test() { Url = baseLink, Links = listLinksForDb };

            _testRepository.Add(test);
            _linkRepository.SaveChanges();
        }
    }
}
