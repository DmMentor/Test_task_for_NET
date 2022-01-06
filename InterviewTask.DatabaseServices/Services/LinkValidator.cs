using System;
using System.Net;

namespace InterviewTask.Logic.Services
{
    public class LinkValidator
    {
        public void CheckLink(Uri link)
        {
            if (link == null)
            {
                throw new ArgumentException("Input value equal null");
            }
            else
            {
                var webClient = new WebClient();
                webClient.DownloadString(link);
            }
        }
    }
}
