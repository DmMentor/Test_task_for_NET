using InterviewTask.Logic.Exceptions;
using System;
using System.Net;

namespace InterviewTask.Logic.Validators
{
    public class LinkValidator
    {
        public void CheckLink(Uri link)
        {
            if (link == null)
            {
                throw new InputLinkInvalidException();
            }

            if (!link.IsAbsoluteUri)
            {
                throw new InputLinkInvalidException();
            }

            var webClient = new WebClient();
            webClient.DownloadString(link);
        }
    }
}
