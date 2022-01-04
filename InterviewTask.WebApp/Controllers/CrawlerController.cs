using InterviewTask.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InterviewTask.WebApp.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly Crawler _webApp;
        private readonly LinkValidator _linkValidator;

        public CrawlerController(Crawler webApp, LinkValidator linkValidator)
        {
            _webApp = webApp;
            _linkValidator = linkValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Input(Uri inputLink)
        {
            var resultTest = _linkValidator.CheckLink(inputLink);

            if (resultTest.IsValid)
            {
                await _webApp.StartAsync(inputLink);
            }

            return RedirectToAction("GetTest", "Database", new { message = resultTest.Message });
        }
    }
}
