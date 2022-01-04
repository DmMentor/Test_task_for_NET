using InterviewTask.Logic.Services;
using InterviewTask.WebApp.Models;
using InterviewTask.WebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.WebApp.Controllers
{
    public class ResultController : Controller
    {
        private readonly DatabaseOperation _databaseOperation;

        public ResultController(DatabaseOperation databaseOperation)
        {
            _databaseOperation = databaseOperation;
        }

        [HttpGet]
        public async Task<IActionResult> GetTest(int page = 1, int pageSize = 3, string message = null)
        {
            var listTests = await _databaseOperation.GetListTestsAsync(page, pageSize);

            var resultTest = new Result()
            {
                Page = page,
                List = listTests.List,
                PageSize = pageSize,
                TotalCount = listTests.TotalCount
            };

            ViewData["message"] = message;

            return View(resultTest);
        }

        [HttpGet]
        public IActionResult GetCrawlingResults(int id)
        {
            var databaseLinks = _databaseOperation.GetListAllLinks(id);

            var resultModel = new ResultView()
            {
                All = databaseLinks.OrderBy(r => r.ResponseTime),
                Html = databaseLinks.Where(s => s.IsLinkFromHtml && !s.IsLinkFromSitemap),
                Sitemap = databaseLinks.Where(s => !s.IsLinkFromHtml && s.IsLinkFromSitemap),
                TotalLinksHtml = databaseLinks.Where(s => s.IsLinkFromHtml).Count(),
                TotalLinksSitemap = databaseLinks.Where(s => s.IsLinkFromSitemap).Count()
            };

            return View(resultModel);
        }
    }
}
