using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Services
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

        public virtual async Task<HttpResponseMessage> GetResponseMessageAsync(Uri link) => await _client.GetAsync(link);

        public virtual async Task<string> GetContentAsync(HttpResponseMessage response)
        {
            using HttpContent content = response.Content;

            return await content.ReadAsStringAsync();
        }
    }
}
