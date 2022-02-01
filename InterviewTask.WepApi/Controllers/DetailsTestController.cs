using InterviewTask.Logic.Models.Logic;
using InterviewTask.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace InterviewTask.WepApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetailsTestController : ControllerBase
    {
        private readonly DatabaseService _databaseOperation;

        public DetailsTestController(DatabaseService databaseOperation)
        {
            _databaseOperation = databaseOperation;
        }

        /// <summary>
        /// Get a list with parsing results links
        /// </summary>
        [HttpGet("{id}")]
        public IEnumerable<Result> GetDetailsTest([FromRoute] int id)
        {
            return _databaseOperation.GetListAllLinks(id);
        }
    }
}
