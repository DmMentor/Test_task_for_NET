using System;
using System.Collections.Generic;

namespace InterviewTask.EntityFramework.Entities
{
    public class Test
    {
        public Test()
        {
            Links = new HashSet<CrawlingResult>();
        }

        public int Id { get; set; }
        public Uri BaseUrl { get; set; }
        public DateTime ParsingDate { get; set; }
        public ICollection<CrawlingResult> Links { get; set; }
    }
}
