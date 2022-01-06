using InterviewTask.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InterviewTask.WebApp.Controllers
{
    public class CrawlerController : Controller
    {
        private readonly WebApplication _webApp;
        private readonly LinkValidator _linkValidator;

        public CrawlerController(WebApplication webApp, LinkValidator linkValidator)
        {
            _webApp = webApp;
            _linkValidator = linkValidator;
        }

        [HttpPost]
        public async Task<IActionResult> Input(Uri inputLink)
        {
            try
            {
                _linkValidator.CheckLink(inputLink);
            }
            catch (Exception ex)
            {
                return RedirectToAction("GetTest", "Result", new { ex.Message });
            }

            await _webApp.StartAsync(inputLink);
            return RedirectToAction("GetTest", "Result", new { message = "Parsing completed successfully" });
        }
    }
}
