using System;
using System.Net;
using System.Text;

namespace Parse_site
{
    class DownloadDocument
    {
        public string Download(Uri inputLink)
        {
            var webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;

            string document;
            
            try
            {
                document = webClient.DownloadString(inputLink);
            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message);
                return null;
            }

            return document;
        }
    }
}
