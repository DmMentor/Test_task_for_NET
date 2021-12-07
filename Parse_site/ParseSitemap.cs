using System;
using System.Collections.Generic;
using ParseDocument;

namespace Parse_site
{
    class ParseSitemap
    {
        private readonly Uri _baseLink;
        private readonly IParseDocument _parseDocument;
        private readonly DownloadDocument _downloadDocument = new DownloadDocument();

        public ParseSitemap(Uri linkInput, IParseDocument parseDocument)
        {
            _baseLink = linkInput;
            _parseDocument = parseDocument;
        }

        public List<Uri> StartParse()
        {
            var linkBuilderSitemap = new UriBuilder(_baseLink.Scheme, _baseLink.Host, _baseLink.Port, "/sitemap.xml");
            Uri linkToDownloadDocument = linkBuilderSitemap.Uri;

            string requestedDocument = _downloadDocument.Download(linkToDownloadDocument);

            List<Uri> listLinkSitemap = _parseDocument.ParseDocument(requestedDocument);

            return listLinkSitemap;
        }
    }
}
