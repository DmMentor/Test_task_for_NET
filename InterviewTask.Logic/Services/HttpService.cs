using System;
using System.Net;
using System.Net.Http;

namespace InterviewTask.Logic.Services
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

        public virtual HttpResponseMessage GetResponseMessage(Uri link)
        {
            try
            {
                return _client.GetAsync(link).Result;
            }
            catch (WebException)
            {
                return new HttpResponseMessage() { StatusCode = HttpStatusCode.RequestTimeout };
            }
        }

        public virtual string GetContent(Uri link)
        {
            var response = GetResponseMessage(link);
            using HttpContent content = response.Content;

            return content.ReadAsStringAsync().Result;
        }
    }
}
