using InterviewTask.EntityFramework.Entities;
using System.Collections.Generic;

namespace InterviewTask.WebApp.ViewModels
{
    public class ResultView
    {
        public IEnumerable<CrawlingResult> All { get; set; }
        public IEnumerable<CrawlingResult> Html { get; set; }
        public IEnumerable<CrawlingResult> Sitemap { get; set; }
        public int TotalLinksHtml { get; set; }
        public int TotalLinksSitemap { get; set; }
    }
}
