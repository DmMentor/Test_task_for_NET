using InterviewTask.Logic.Parser;
using InterviewTask.Logic.Services;

namespace InterviewTask.Logic.Crawler
{
    public class WebsiteCrawlerBuild
    {
        public WebsiteCrawler Build()
        {
            var parseDocumentHtml = new ParseDocumentHtml();
            var parseDocumentXml = new ParseDocumentSitemap();
            var downloadDocument = new DownloadDocument();
            var convertLink = new ConvertLink();

            var parseHtml = new HtmlCrawler(parseDocumentHtml, downloadDocument, convertLink);
            var parseSitemap = new SitemapCrawler(parseDocumentXml, downloadDocument);

            var crawler = new WebsiteCrawler(parseHtml, parseSitemap, downloadDocument);

            return crawler;
        }
    }
}
