using System.Collections.Generic;
using System.Linq;

namespace InterviewTask.Logic.Parser
{
    public class ParseDocumentHtml
    {
        public virtual IEnumerable<string> ParseDocument(string document)
        {
            string conditionalLink = "href=\"";

            IEnumerable<string> listLinks = document?.Split('\n', '\r')
                .Where(d => d.Contains(conditionalLink))
                .Select(d => CutLinkString(d, conditionalLink));

            return listLinks;
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