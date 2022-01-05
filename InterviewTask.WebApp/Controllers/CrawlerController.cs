using InterviewTask.Logic.Services;
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
            string message = "Parsing completed successfully";

            try
            {
                _linkValidator.CheckLink(inputLink);
                await _webApp.StartAsync(inputLink);
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return RedirectToAction("GetTest", "Result", new { message });
        }
    }
}
