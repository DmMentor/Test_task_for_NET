using InterviewTask.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InterviewTask.WebApp.Controllers
{
    public class ResultController : Controller
    {
        private readonly DatabaseService _databaseOperation;

        public ResultController(DatabaseService databaseOperation)
        {
            _databaseOperation = databaseOperation;
        }

        [HttpGet]
        public async Task<IActionResult> GetTest(int page = 1, int pageSize = 3, string message = null)
        {
            var resultTest = await _databaseOperation.GetPageTestsAsync(page, pageSize);

            ViewData["message"] = message;

            return View(resultTest);
        }

        [HttpGet]
        public IActionResult GetCrawlingResults(int id)
        {
            var linksResult = _databaseOperation.GetListAllLinks(id);

            return View(linksResult);
        }
    }
}
