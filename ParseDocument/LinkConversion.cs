using System;
using System.Linq;

namespace ParseDocument
{
    public class LinkConversion
    {
        private readonly Uri _baseLink;
        public LinkConversion(Uri link)
        {
            _baseLink = link;
        }

        public Uri Converting(string inputLink)
        {
            string formattedLink = string.Empty;
            string baseStartLink = _baseLink.Scheme + "://" + _baseLink.Host;
            
            Uri link;

            if (inputLink.Length <= 1 || inputLink.Contains("/?") || inputLink.Contains("#"))
            {
                link = new Uri(baseStartLink);
            }
            else if (inputLink.StartsWith("http"))
            {
                link = new Uri(inputLink);

                return link.Host == _baseLink.Host ? link : null;
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
    }
}
