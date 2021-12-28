using System;

namespace InterviewTask.CrawlerServices.Models
{
    public class LinkWithResponse
    {
        public Uri Url { get; set; }
        public int ResponseTime { get; set; }
    }
}
