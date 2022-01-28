using System.Collections.Generic;

namespace InterviewTask.Logic.Models.Logic
{
    public class ResultPagination<T> : Pagination
    {
        public IEnumerable<T> List { get; set; }
    }
}
