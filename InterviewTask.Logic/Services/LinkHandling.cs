using System;
using System.Net;
using System.Net.Http;

namespace InterviewTask.Logic.Services
{
    public class LinkHandling
    {
        private readonly int _timeout;

        public LinkHandling(int timeout = 10000)
        {
            _timeout = timeout;
        }

        public virtual HttpResponseMessage GetLinkResponse(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(_timeout);

            try
            {
                return client.GetAsync(inputLink).Result;
            }
            catch (WebException)
            {
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.RequestTimeout };
            }
        }

        public virtual string DownloadDocument(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var response = GetLinkResponse(inputLink);

            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            using HttpContent content = response.Content;

            return content.ReadAsStringAsync().Result;
        }
    }
}
