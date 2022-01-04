using InterviewTask.CrawlerLogic.Parsers;
using InterviewTask.CrawlerLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Crawlers
{
    public class HtmlCrawler
    {
        private readonly ParseDocumentHtml _parseDocument;
        private readonly LinkHandling _linkHandling;
        private readonly Converter _converter;

        public HtmlCrawler(ParseDocumentHtml parseDocument, LinkHandling linkHandling, Converter converter)
        {
            _parseDocument = parseDocument;
            _linkHandling = linkHandling;
            _converter = converter;
        }

        public virtual async Task<IEnumerable<Uri>> StartParseAsync(Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var listLinksHtml = new List<Uri>();
            listLinksHtml.Add(baseLink);

            var parseLinksQueue = new Queue<Uri>();
            parseLinksQueue.Enqueue(baseLink);

            while (parseLinksQueue.Count > 0)
            {
                var linkToParse = parseLinksQueue.Dequeue();

                string documentHtml = await _linkHandling.DownloadDocumentAsync(linkToParse);

                if (string.IsNullOrWhiteSpace(documentHtml))
                {
                    continue;
                }

                var listUriLinks = ParseDocumentToLinks(documentHtml, baseLink).Except(listLinksHtml);

                foreach (var link in listUriLinks)
                {
                    listLinksHtml.Add(link);
                    parseLinksQueue.Enqueue(link);
                }
            }

            return listLinksHtml;
        }

        private IEnumerable<Uri> ParseDocumentToLinks(string documentHtml, Uri baseLink)
        {
            var listStringLinks = _parseDocument.ParseDocument(documentHtml);

            if (!listStringLinks.Any())
            {
                return Enumerable.Empty<Uri>();
            }

            return listStringLinks.Select(link => _converter.ToUri(link, baseLink))
                .Where(link => link != null);
        }
    }
}
