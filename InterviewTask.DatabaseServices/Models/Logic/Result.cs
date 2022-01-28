using System;

namespace InterviewTask.Logic.Models.Logic
{
    public class Result
    {
        public Uri Url { get; set; }
        public int ResponseTime { get; set; }
        public bool IsLinkFromHtml { get; set; }
        public bool IsLinkFromSitemap { get; set; }
    }
}