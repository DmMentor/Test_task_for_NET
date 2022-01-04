using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Services
{
    public class LinkHandling
    {
        private readonly HttpService _httpService;

        public LinkHandling(HttpService httpService)
        {
            _httpService = httpService;
        }

        public virtual async Task<int> GetLinkResponseAsync(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var timer = Stopwatch.StartNew();
            var responseMessage = await _httpService.GetResponseMessageAsync(inputLink);
            timer.Stop();

            if (responseMessage.IsSuccessStatusCode)
            {
                return timer.Elapsed.Milliseconds;
            }

            return int.MaxValue;
        }

        public virtual async Task<string> DownloadDocumentAsync(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var responseMessage = await _httpService.GetResponseMessageAsync(inputLink);
            if (responseMessage.IsSuccessStatusCode)
            {
                return await _httpService.GetContentAsync(responseMessage);
            }

            return string.Empty;
        }
    }
}
