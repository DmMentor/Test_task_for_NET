using InterviewTask.Logic.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Crawler
{
    public class HtmlCrawler
    {
        private readonly ParseDocumentHtml _parseDocument;
        private readonly DownloadDocument _downloadDocument;
        private readonly ConvertLink _convertLink;

        public HtmlCrawler(ParseDocumentHtml parseDocument, DownloadDocument downloadDocument, ConvertLink convertLink)
        {
            _parseDocument = parseDocument;
            _downloadDocument = downloadDocument;
            _convertLink = convertLink;
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

                string documentHtml = _downloadDocument.Download(linkToParse);

                if (string.IsNullOrEmpty(documentHtml))
                {
                    continue;
                }

                var lisitStringLinks = _parseDocument.ParseDocument(documentHtml);

                if (!lisitStringLinks.Any())
                {
                    continue;
                }

                var listUriLinks = lisitStringLinks.Select(l => _convertLink.ConvertStringToUri(l, baseLink)).Except(listLinksHtml).Where(l => l != null);

                foreach (var link in listUriLinks)
                {
                    if (!listLinksHtml.Contains(link))
                    {
                        listLinksHtml.Add(link);
                        parseLinksQueue.Enqueue(link);
                    }
                }
            }

            return listLinksHtml;
        }
    }
}
