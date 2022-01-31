using InterviewTask.Logic.Models.Logic;
using InterviewTask.Logic.Models.Request;
using InterviewTask.Logic.Services;
using InterviewTask.Logic.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewTask.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DatabaseService _databaseOperation;
        private readonly WebApplication _webApp;
        private readonly LinkValidator _linkValidator;

        public TestController(DatabaseService databaseOperation, WebApplication webApp, LinkValidator linkValidator)
        {
            _webApp = webApp;
            _linkValidator = linkValidator;
            _databaseOperation = databaseOperation;
        }

        /// <summary>
        /// Get the tests page
        /// </summary>
        [HttpGet]
        public async Task<ResultPagination<TestModel>> GetTest([FromQuery] GetTestRequest getTestRequest)
        {
            return await _databaseOperation.GetPageTestsAsync(getTestRequest.CurrentPage, getTestRequest.PageSize);
        }

        /// <summary>
        /// Get a list with parsing results links
        /// </summary>
        [HttpGet("DetailsTest/{testId}")]
        public IEnumerable<Result> GetDetailsTest([FromRoute] int testId)
        {
            return _databaseOperation.GetListAllLinks(testId);
        }

        /// <summary>
        /// Start link parsing
        /// </summary>
        /// <response code="200">Parsing of the reference has been initiated and successfully completed</response>
        /// <response code="400">Link is invalid</response>
        [HttpPost]
        public async Task<ActionResult> CreateTest([FromForm] InputLinkRequest inputLinkRequest)
        {
            _linkValidator.CheckLink(inputLinkRequest.Link);
   
            await _webApp.StartAsync(inputLinkRequest.Link);

            return Ok("Parsing completed successfully");
        }
    }
}
