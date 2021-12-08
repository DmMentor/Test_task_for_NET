using System;
using System.Net.Http;
using System.Threading;

namespace Parse_site
{
    class DownloadDocument
    {
        public string Download(Uri inputLink)
        {
            string document = string.Empty;

            Thread.Sleep(100);

            HttpClient client = new HttpClient();

            using (HttpResponseMessage response = client.GetAsync(inputLink).Result)
            {
                using (HttpContent content = response.Content)
                {
                    document = content.ReadAsStringAsync().Result;
                }
            }

            return document;
        }
    }
}
