namespace InterviewTask.Logic.Models.Logic
{
    public abstract class Pagination
    {
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
    }
}