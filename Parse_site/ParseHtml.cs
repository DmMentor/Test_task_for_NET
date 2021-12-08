using System;
using System.Collections.Generic;
using ParseDocument;

namespace Parse_site
{
    class ParseHtml
    {
        private readonly Uri _baseLink;
        private readonly IParseDocument _parseDocument;
        private readonly DownloadDocument _downloadDocument = new DownloadDocument();

        public ParseHtml(Uri link, IParseDocument parseDocument)
        {
            _baseLink = link;
            _parseDocument = parseDocument;
        }

        public List<Uri> StartParse()
        {
            var listLinksHtml = new List<Uri>();

            RecursiveWebsiteParse(listLinksHtml, _baseLink);

            return listLinksHtml;
        }

        private void RecursiveWebsiteParse(List<Uri> listLinksHtml, Uri startLink)
        {
            string documentHtml = _downloadDocument.Download(startLink);

            if (documentHtml == null)
            {
                return;
            }

            List<Uri> listLinksFromHtml = _parseDocument.ParseDocument(documentHtml);

            var listNewLinks = new List<Uri>();

            foreach (var link in listLinksFromHtml)
            {
                if (link != null && !listLinksHtml.Contains(link))
                {
                    listNewLinks.Add(link);
                    listLinksHtml.Add(link);
                }
            }

            foreach (var newLink in listNewLinks)
            {
                if (newLink != startLink)
                {
                    RecursiveWebsiteParse(listLinksHtml, newLink);
                }
            }
        }
    }
}
