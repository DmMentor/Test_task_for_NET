using InterviewTask.CrawlerLogic.Parsers;
using InterviewTask.CrawlerLogic.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Crawlers
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

        public virtual async Task<IEnumerable<Uri>> ParseAsync(Uri baseLink)
        {
            if (!baseLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var linkBuilderSitemap = new UriBuilder(baseLink.Scheme, baseLink.Host, baseLink.Port, "/sitemap.xml");
            Uri linkToDownloadDocument = linkBuilderSitemap.Uri;

            var requestedDocument = await _linkHandling.DownloadDocumentAsync(linkToDownloadDocument);

            var listLinkSitemap = _parseDocument.ParseDocument(requestedDocument);

            return listLinkSitemap;
        }
    }
}
