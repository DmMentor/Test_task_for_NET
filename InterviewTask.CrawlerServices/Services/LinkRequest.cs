using InterviewTask.CrawlerLogic.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterviewTask.CrawlerLogic.Services
{
    public class LinkRequest
    {
        private readonly LinkHandling _linkHandling;

        public LinkRequest(LinkHandling linkHandling)
        {
            _linkHandling = linkHandling;
        }

        public async Task<IEnumerable<LinkWithResponse>> GetListWithLinksResponseTimeAsync(IEnumerable<Link> inputListLinks)
        {
            if (inputListLinks == null)
            {
                throw new ArgumentNullException(nameof(inputListLinks), "List is null");
            }

            var listLinksWithResponse = new List<LinkWithResponse>();

            foreach (var link in inputListLinks)
            {
                if (link.Url != null)
                {
                    var linkResponse = new LinkWithResponse
                    {
                        Url = link.Url,
                        ResponseTime = await _linkHandling.GetLinkResponseAsync(link.Url)
                    };

                    listLinksWithResponse.Add(linkResponse);
                }
            }

            return listLinksWithResponse;
        }
    }
}
