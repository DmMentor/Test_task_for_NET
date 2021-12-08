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

        public HtmlCrawler(ParseDocumentHtml parseDocument, DownloadDocument downloadDocument)
        {
            _parseDocument = parseDocument;
            _downloadDocument = downloadDocument;
        }

        public IEnumerable<Uri> StartParse(Uri baseLink)
        {
            var listLinksHtml = new List<Uri>();

            var parseLinksQueue = new Queue<Uri>();
            parseLinksQueue.Enqueue(baseLink);

            while (parseLinksQueue.Count > 0)
            {
                var linkToParse = parseLinksQueue.Dequeue();

                string documentHtml = _downloadDocument.Download(linkToParse);

                if (documentHtml.Length > 0)
                {
                    continue;
                }

                var listLinksFromHtml = _parseDocument.ParseDocument(documentHtml, baseLink).Except(listLinksHtml).Where(l => l != null);

                foreach (var link in listLinksFromHtml)
                {
                    listLinksHtml.Add(link);
                    parseLinksQueue.Enqueue(link);
                }
            }

            return listLinksHtml;
        }
    }
}
