using System;
using System.Net;

namespace Parse_site.Download
{
    class DownloadDocument
    {
        public string Download(Uri inputLink)
        {
            var webClient = new WebClient();
            webClient.Encoding = System.Text.Encoding.UTF8;

            string document;

            try
            {
                document = webClient.DownloadString(inputLink);
            }
            catch (WebException)
            {
                return null;
            }

            return document;
        }
    }
}
