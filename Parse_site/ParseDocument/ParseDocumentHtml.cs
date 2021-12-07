using System.Collections.Generic;
using Parse_site.Download;

namespace Parse_site.ParseDocument
{
    class ParseDocumentHtml : IParseDocument
    {
        public List<string> ParseDocument<T>(string inputLink, IDownloadDocument<T> download) where T : class
        {
            List<string> listLinesDocument = ((DownloadDocumentHtml)download).DownloadDocument(inputLink);

            List<string> res3 = new List<string>();

            string conditionalLink = "href=\"";

            foreach (var lineInDocument in listLinesDocument)
            {
                if (lineInDocument.Contains("<a"))
                {
                    int startIndex = lineInDocument.IndexOf(conditionalLink);

                    if (startIndex > -1)
                    {
                        string linkWithCroppedStart = lineInDocument.Substring(startIndex + conditionalLink.Length);
                        int lengthMainLink = linkWithCroppedStart.IndexOf("\"");
                        string linkResult = linkWithCroppedStart.Substring(0, lengthMainLink);

                        res3.Add(linkResult);
                    }
                }
            }

            return res3;
        }
    }
}
