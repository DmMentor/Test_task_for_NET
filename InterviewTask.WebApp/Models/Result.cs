using InterviewTask.EntityFramework.Entities;
using System.Collections.Generic;

namespace InterviewTask.WebApp.Models
{
    public class Result
    {
        public IEnumerable<Test> List { get; set; }
        public int Page { get; set; } = 1;
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public bool IsLastPage
        {
            get => PageSize * Page >= TotalCount;
        }
        public bool IsFirstPage
        {
            get => Page <= 1;
        }
    }
}
