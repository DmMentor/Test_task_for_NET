using System;
using ParseDocument;

namespace Parse_site
{
    class CrawlerBuild
    {
        public Crawler Build(string link)
        {
            var linkUri = new Uri(link);

            var linkRequest = new LinkRequest();

            if(linkRequest.SendRequest(linkUri) == 0)
            {
                throw new Exception("Link is invalid");
            }

            var parseDocumentHtml = new ParseDocumentHtml(linkUri);
            var parseDocumentXml = new ParseDocumentXml();

            var parseHtml = new ParseHtml(linkUri, parseDocumentHtml);
            var parseSitemap = new ParseSitemap(linkUri, parseDocumentXml);
            
            var crawler = new Crawler(parseHtml, parseSitemap, linkRequest);
            
            return crawler;
        }
    }
}
