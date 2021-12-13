using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace InterviewTask.Logic.Services
{
    public class LinkRequest
    {
        public virtual int LinkResponseTime(Uri link, int timeout = 10000)
        {
            if (!link.IsAbsoluteUri)
            {
                throw new ArgumentException("Link must be absolute");
            }

            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

            try
            {
                var timer = Stopwatch.StartNew();

                using HttpResponseMessage response = httpClient.GetAsync(link).Result;

                timer.Stop();

                return timer.Elapsed.Milliseconds;
            }
            catch (WebException)
            {
                return 0;
            }
        }
    }
}
