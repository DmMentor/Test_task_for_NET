using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Parser
{
    public class ParseDocumentHtml
    {
        public IEnumerable<Uri> ParseDocument(string document, Uri baseLink)
        {
            string conditionalLink = "href=\"";

            IEnumerable<Uri> listLinks = document.Split('\n', '\r')
                .Where(d => d.Contains("<a") && d.Contains(conditionalLink))
                .Select(d => ConvertStringToUri(d, conditionalLink, baseLink));

            return listLinks;
        }

        private Uri ConvertStringToUri(string inputLink, string conditionalLink, Uri baseLink)
        {
            string cutLink = CutLinkString(inputLink, conditionalLink);

            string baseStartLink = baseLink.Scheme + "://" + baseLink.Host;

            Uri link = null;

            if (cutLink.Length <= 1 || cutLink.Contains("/?") || cutLink.Contains("#"))
            {
                link = new Uri(baseStartLink);
            }
            else if (cutLink.StartsWith("http"))
            {
                link = new Uri(cutLink);

                return link.Host == baseLink.Host ? link : null;
            }
            else
            {
                if (cutLink.Where(s => s == ':').Count() > 0)
                {
                    return null;
                }

                string absolutePath = cutLink.StartsWith('/') ? cutLink : '/' + cutLink;

                link = new Uri(baseStartLink + absolutePath);
            }

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