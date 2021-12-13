using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Crawlers
{
    public class HtmlCrawler
    {
        private readonly ParseDocumentHtml _parseDocument;
        private readonly LinkHandling _downloadDocument;
        private readonly Converter _converter;

        public HtmlCrawler(ParseDocumentHtml parseDocument, LinkHandling downloadDocument, Converter converter)
        {
            _parseDocument = parseDocument;
            _downloadDocument = downloadDocument;
            _converter = converter;
        }

        public IEnumerable<Uri> StartParse(Uri baseLink)
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

                string documentHtml = _downloadDocument.DownloadDocument(linkToParse);

                if (string.IsNullOrEmpty(documentHtml))
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

        public IEnumerable<Uri> ParseDocumentToLinks(string documentHtml, Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            if (string.IsNullOrEmpty(documentHtml))
            {
                return Enumerable.Empty<Uri>();
            }

            var listStringLinks = _parseDocument.ParseDocument(documentHtml);

            if (!listStringLinks.Any())
            {
                return Enumerable.Empty<Uri>();
            }

            return listStringLinks.Select(link => _converter.ToUri(link, baseLink)).Where(link => link != null);
        }
    }
}
