using System;
using System.Diagnostics;

namespace InterviewTask.Logic.Services
{
    public class LinkHandling
    {
        private readonly HttpService _httpService;

        public LinkHandling(HttpService httpService)
        {
            _httpService = httpService;
        }

        public virtual int GetLinkResponse(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var timer = Stopwatch.StartNew();
            var responseMessage = _httpService.GetResponseMessage(inputLink);
            timer.Stop();

            if (responseMessage.IsSuccessStatusCode)
            {
                return timer.Elapsed.Milliseconds;
            }

            return int.MaxValue;
        }

        public virtual string DownloadDocument(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            var responseMessage = _httpService.GetResponseMessage(inputLink);
            if (responseMessage.IsSuccessStatusCode)
            {
                return _httpService.GetContent(responseMessage);
            }

            return string.Empty;
        }
    }
}
