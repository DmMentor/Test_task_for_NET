using System;
using System.Net.Http;

namespace InterviewTask.CrawlerServices.Services
{
    public class HttpService
    {
        private readonly HttpClient _client;

        public HttpService(int timeout = 15000)
        {
            _client = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };
        }

        public virtual HttpResponseMessage GetResponseMessage(Uri link) => _client.GetAsync(link).Result;

        public virtual string GetContent(HttpResponseMessage response)
        {
            using HttpContent content = response.Content;

            return content.ReadAsStringAsync().Result;
        }
    }
}
