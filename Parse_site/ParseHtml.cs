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

            RecursiveParse(listLinksHtml, _baseLink);

            return listLinksHtml;
        }

        private void RecursiveParse(List<Uri> listOldLinks, Uri startLink)
        {
            string document;
            List<Uri> listLinksFromHtml = null;

            try
            {
                document = _downloadDocument.Download(startLink) ?? throw new Exception($"Document html empty for url: {startLink}");
                listLinksFromHtml = _parseDocument.ParseDocument(document);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (listLinksFromHtml == null)
            {
                return;
            }

            var listNewLinks = new List<Uri>();

            foreach (var link in listLinksFromHtml)
            {
                if (link != null && !listOldLinks.Contains(link))
                {
                    listNewLinks.Add(link);
                    listOldLinks.Add(link);
                }
            }

            foreach (var newLink in listNewLinks)
            {
                if (newLink != startLink)
                {
                    Console.WriteLine(newLink);
                    RecursiveParse(listOldLinks, newLink);
                }
            }
        }
    }
}
