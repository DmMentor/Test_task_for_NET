using System;

namespace InterviewTask.Logic.Models
{
    public class Link
    {
        public Uri Url { get; set; }
        public bool IsLinkFromHtml { get; set; }
        public bool IsLinkFromSitemap { get; set; }
    }
}
