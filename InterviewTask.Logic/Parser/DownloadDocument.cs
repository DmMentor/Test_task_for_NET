using System;
using System.Net;
using System.Net.Http;

namespace InterviewTask.Logic.Parser
{
    public class DownloadDocument
    {
        public virtual string Download(Uri inputLink, int timeout = 10000)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            HttpClient client = new HttpClient();

            client.Timeout = TimeSpan.FromMilliseconds(timeout);

            try
            {
                using HttpResponseMessage response = client.GetAsync(inputLink).Result;
                using HttpContent content = response.Content;

                return content.ReadAsStringAsync().Result;
            }
            catch (WebException)
            {
                return string.Empty;
            }
        }
    }
}
