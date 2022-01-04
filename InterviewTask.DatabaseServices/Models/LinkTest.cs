using InterviewTask.EntityFramework.Entities;
using System.Collections.Generic;

namespace InterviewTask.Logic.Models
{
    public class LinkTest
    {
        public IEnumerable<Test> List { get; set; }
        public int TotalCount { get; set; }
    }
}
