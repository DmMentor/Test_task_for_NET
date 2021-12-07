using System;
using System.Collections.Generic;
using System.Linq;

namespace Parse_site.ParseDocument
{
    class ParseDocumentHtml : IParseDocument
    {
        public List<Uri> ParseDocument(string document)
        {
            string conditionalLink = "href=\"";

            List<string> listLinesDocument = document.Split('\n', '\r')
                .Where(d => d.Contains("<a") && d.Contains(conditionalLink))
                .Select(d => CutLinkString(d, conditionalLink))
                .ToList();

            return listLinesDocument;
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