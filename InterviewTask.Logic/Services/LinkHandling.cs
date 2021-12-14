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

            return -1;
        }

        public virtual string DownloadDocument(Uri inputLink)
        {
            if (!inputLink.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            string document = _httpService.GetContent(inputLink);

            return document;
        }
    }
}
