using InterviewTask.CrawlerServices.Parsers;
using InterviewTask.CrawlerServices.Services;
using System;
using System.Collections.Generic;

namespace InterviewTask.CrawlerServices.Crawlers
{
    public class SitemapCrawler
    {
        private readonly ParseDocumentSitemap _parseDocument;
        private readonly LinkHandling _linkHandling;

        public SitemapCrawler(ParseDocumentSitemap parseDocument, LinkHandling linkHandling)
        {
            _parseDocument = parseDocument;
            _linkHandling = linkHandling;
        }

        public virtual IEnumerable<Uri> Parse(Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var linkBuilderSitemap = new UriBuilder(baseLink.Scheme, baseLink.Host, baseLink.Port, "/sitemap.xml");
            Uri linkToDownloadDocument = linkBuilderSitemap.Uri;

            var requestedDocument = _linkHandling.DownloadDocument(linkToDownloadDocument);

            var listLinkSitemap = _parseDocument.ParseDocument(requestedDocument);

            return listLinkSitemap;
        }
    }
}
