using InterviewTask.Logic.Parser;
using System;
using System.Collections.Generic;

namespace InterviewTask.Logic.Crawler
{
    public class SitemapCrawler
    {
        private readonly ParseDocumentSitemap _parseDocument;
        private readonly DownloadDocument _downloadDocument;

        public SitemapCrawler(ParseDocumentSitemap parseDocument, DownloadDocument downloadDocument)
        {
            _parseDocument = parseDocument;
            _downloadDocument = downloadDocument;
        }

        public IEnumerable<Uri> Parse(Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var linkBuilderSitemap = new UriBuilder(baseLink.Scheme, baseLink.Host, baseLink.Port, "/sitemap.xml");
            Uri linkToDownloadDocument = linkBuilderSitemap.Uri;

            string requestedDocument = _downloadDocument.Download(linkToDownloadDocument);

            IEnumerable<Uri> listLinkSitemap = _parseDocument.ParseDocument(requestedDocument);

            return listLinkSitemap ?? Array.Empty<Uri>();
        }
    }
}
