using System;
using System.Collections.Generic;
using System.Linq;

namespace ParseDocument
{
    public class ParseDocumentHtml : IParseDocument
    {
        private readonly LinkConversion linkConversion;

        public ParseDocumentHtml(Uri link)
        {
            linkConversion = new LinkConversion(link);
        }

        public List<Uri> ParseDocument(string document)
        {
            string conditionalLink = "href=\"";

            List<Uri> listLinks = document.Split('\n', '\r')
                .Where(d => d.Contains("<a") && d.Contains(conditionalLink))
                .Select(d => CutLinkString(d, conditionalLink))
                .Select(d => ConvertStringToUri(d))
                .ToList();

            return listLinks;
        }

        private Uri ConvertStringToUri(string inputLink)
        {
            Uri link = linkConversion.Converting(inputLink);

            return link;
        }

        private string CutLinkString(string link, string conditionalLink)
        {
            int startIndex = link.IndexOf(conditionalLink);

            string linkWithCroppedStart = link.Substring(startIndex + conditionalLink.Length);
            int lengthMainLink = linkWithCroppedStart.IndexOf("\"");
            string linkResult = linkWithCroppedStart.Substring(0, lengthMainLink);

            return linkResult;
        }
    }
}