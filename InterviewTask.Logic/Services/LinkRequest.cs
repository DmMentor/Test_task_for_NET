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

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(timeout);

            try
            {
                var time = Stopwatch.StartNew();

                using HttpResponseMessage response = client.GetAsync(link).Result;

                time.Stop();

                return time.Elapsed.Milliseconds;
            }
            catch (WebException)
            {
                return 0;
            }
        }
    }
}
