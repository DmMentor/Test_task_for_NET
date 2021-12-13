using InterviewTask.Logic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace InterviewTask.Logic.Services
{
    public class LinkRequest
    {
        private readonly LinkHandling _linkHandling;

        public LinkRequest(LinkHandling linkHandling)
        {
            _linkHandling = linkHandling;
        }

        public IEnumerable<LinkWithResponse> GetListWithLinksResponseTime(IEnumerable<Link> inputListLinks)
        {
            if (inputListLinks == null)
            {
                throw new ArgumentNullException("inputList", "List is null");
            }

            var listLinksWithResponse = new List<LinkWithResponse>();

            foreach (var link in inputListLinks)
            {
                LinkWithResponse linkResponse = new LinkWithResponse();
                linkResponse.Url = link.Url;

                var timer = Stopwatch.StartNew();
                _linkHandling.GetLinkResponse(link.Url);
                timer.Stop();

                linkResponse.ResponseTime = timer.Elapsed.Milliseconds;

                listLinksWithResponse.Add(linkResponse);
            }

            return listLinksWithResponse;
        }
    }
}
