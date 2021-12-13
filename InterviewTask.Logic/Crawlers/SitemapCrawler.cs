using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;

namespace InterviewTask.Logic.Crawlers
{
    public class SitemapCrawler
    {
        private readonly ParseDocumentSitemap _parseDocument;
        private readonly LinkHandling _linkHandling;

        public SitemapCrawler(ParseDocumentSitemap parseDocument, LinkHandling downloadDocument)
        {
            _parseDocument = parseDocument;
            _linkHandling = downloadDocument;
        }

        public IEnumerable<Uri> Parse(Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var linkBuilderSitemap = new UriBuilder(baseLink.Scheme, baseLink.Host, baseLink.Port, "/sitemap.xml");
            Uri linkToDownloadDocument = linkBuilderSitemap.Uri;

            string requestedDocument = _linkHandling.DownloadDocument(linkToDownloadDocument);

            IEnumerable<Uri> listLinkSitemap = _parseDocument.ParseDocument(requestedDocument);

            return listLinkSitemap ?? Array.Empty<Uri>();
        }
    }
}
