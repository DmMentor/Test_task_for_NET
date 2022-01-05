using System.Collections.Generic;

namespace InterviewTask.Logic.Models
{
    public class ResultPagination<T> : Pagination
    {
        public IEnumerable<T> List { get; set; }
    }
}
