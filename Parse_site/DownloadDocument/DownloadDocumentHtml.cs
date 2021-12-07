using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Parse_site.Download
{
    class DownloadDocumentHtml : IDownloadDocument<List<string>>
    {
        public List<string> DownloadDocument(string inputLink)
        {
            List<string> htmlDocument = new List<string>();

            HttpClient client = new HttpClient();

            HttpResponseMessage http = client.GetAsync(inputLink).GetAwaiter().GetResult();

            using (StreamReader streamToDocumentHtml = new StreamReader(http.Content.ReadAsStreamAsync().Result))
            {
                while (true)
                {
                    string line = streamToDocumentHtml.ReadLine();

                    if (line != null)
                    {
                        htmlDocument.Add(line);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return htmlDocument;
        }
    }
}
