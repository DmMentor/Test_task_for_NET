using System;

namespace InterviewTask.EntityFramework.Entities
{
    public class CrawlingResult
    {
        public int Id { get; set; }
        public Uri Url { get; set; }
        public int ResponseTime { get; set; }
        public bool IsLinkFromHtml { get; set; }
        public bool IsLinkFromSitemap { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
