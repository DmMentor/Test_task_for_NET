using InterviewTask.EntityFramework.Entities;
using System.Collections.Generic;

namespace InterviewTask.Logic.Models
{
    public class ResultPartPage
    {
        public IEnumerable<Test> List { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
    }
}
