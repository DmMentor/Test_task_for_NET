using System;
using System.Net.Http;

namespace InterviewTask.Logic.Parser
{
    public class DownloadDocument
    {

        public string Download(Uri inputLink, int timeout = 10000)
        {
            string document;

            HttpClient client = new HttpClient();

            client.Timeout = TimeSpan.FromMilliseconds(timeout);

            using (HttpResponseMessage response = client.GetAsync(inputLink).Result)
            {
                using (HttpContent content = response.Content)
                {
                    document = content.ReadAsStringAsync().Result;
                }
            }

            return document ?? "";
        }
    }
}
