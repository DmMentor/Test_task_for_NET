using InterviewTask.Logic.Models;
using InterviewTask.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace InterviewTask.Logic.Parser
{
    public class Converter
    {
        private readonly LinkRequest _linkRequest;
        private readonly int _timeSleep;

        public Converter(LinkRequest linkRequest, int timeSleep = 150)
        {
            _linkRequest = linkRequest;
            _timeSleep = timeSleep;
        }

        public virtual Uri ToUri(string inputLink, Uri baseLink)
        {
            string baseStartLink = baseLink.Scheme + "://" + baseLink.Host;

            Uri link = null;

            if (inputLink.Length <= 1 || inputLink.Contains("/?") || inputLink.Contains("#"))
            {
                link = new Uri(baseStartLink);
            }
            else if (inputLink.StartsWith("http"))
            {
                link = new Uri(inputLink);

                return link.Host == baseLink.Host ? link : null;
            }
            else
            {
                if (inputLink.Where(s => s == ':').Count() > 0)
                {
                    return null;
                }

                string absolutePath = inputLink.StartsWith('/') ? inputLink : '/' + inputLink;

                link = new Uri(baseStartLink + absolutePath);
            }

            return link;
        }

        public IEnumerable<LinkWithResponse> ToLinkWithResponse(IEnumerable<Link> inputList)
        {
            if (inputList == null)
            {
                throw new ArgumentNullException("inputList", "List is null");
            }

            var newList = new List<LinkWithResponse>();

            foreach (Link link in inputList)
            {
                LinkWithResponse linkResponse = new LinkWithResponse();
                linkResponse.Url = link.Url;
                linkResponse.IsLinkFromHtml = link.IsLinkFromHtml;
                linkResponse.IsLinkFromSitemap = link.IsLinkFromSitemap;

                Thread.Sleep(_timeSleep);
                linkResponse.ResponseTime = _linkRequest.LinkResponseTime(linkResponse.Url);

                newList.Add(linkResponse);
            }

            return newList;
        }
    }
}
