using InterviewTask.LogicCrawler.Models;
using System;
using System.Collections.Generic;

namespace InterviewTask.LogicCrawler.Services
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
                throw new ArgumentNullException(nameof(inputListLinks), "List is null");
            }

            var listLinksWithResponse = new List<LinkWithResponse>();

            foreach (var link in inputListLinks)
            {
                var linkResponse = new LinkWithResponse
                {
                    Url = link.Url,
                    ResponseTime = _linkHandling.GetLinkResponse(link.Url)
                };

                listLinksWithResponse.Add(linkResponse);
            }

            return listLinksWithResponse;
        }
    }
}
