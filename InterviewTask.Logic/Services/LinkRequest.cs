using InterviewTask.Logic.Models;
using System;
using System.Collections.Generic;

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
                throw new ArgumentNullException(nameof(inputListLinks), "List is null");
            }

            var listLinksWithResponse = new List<LinkWithResponse>();

            foreach (var link in inputListLinks)
            {
                LinkWithResponse linkResponse = new LinkWithResponse();

                linkResponse.Url = link.Url;
                linkResponse.ResponseTime = _linkHandling.GetLinkResponse(link.Url);
                
                listLinksWithResponse.Add(linkResponse);
            }

            return listLinksWithResponse;
        }
    }
}
