using InterviewTask.DatabaseServices.Services;
using InterviewTask.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace InterviewTask.WebApp.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly DatabaseOperation _databaseOperation;
        private const int pageSize = 30;

        public DatabaseController(DatabaseOperation databaseOperation)
        {
            _databaseOperation = databaseOperation;
        }

        [HttpGet]
        public IActionResult GetTest(int page = 1)
        {
            var listTests = _databaseOperation.GetListTests();
            return View(listTests.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public IActionResult GetCrawlingResultsAllLink(int id, int page = 1)
        {
            var listLinksById = _databaseOperation.GetListAllLinks(id).OrderBy(i => i.ResponseTime);
            return View(listLinksById.ToPagedList(page, pageSize));
        }

        [HttpGet]
        public IActionResult GetCrawlingResultsLinkByFlag(int id, bool isHtmlFlag, int page = 1)
        {
            IEnumerable<CrawlingResult> listLinksById;

            if (isHtmlFlag)
            {
                listLinksById = _databaseOperation.GetListHtmlLinks(id);
            }
            else
            {
                listLinksById = _databaseOperation.GetListSitemapLinks(id);
            }

            return View(listLinksById.ToPagedList(page, pageSize));
        }
    }
}
