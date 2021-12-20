using System;
using System.Linq;

namespace InterviewTask.Logic.Services
{
    public class Converter
    {
        public virtual Uri ToUri(string inputLink, Uri baseLink)
        {
            string baseStartLink = baseLink.Scheme + "://" + baseLink.Host;

            if (inputLink.Length <= 1 || inputLink.Contains("/?") || inputLink.Contains("#"))
            {
                return new Uri(baseStartLink);
            }

            if (inputLink.StartsWith("http"))
            {
                var link = new Uri(inputLink);

                return link.Host == baseLink.Host ? link : null;
            }

            if (inputLink.Any(s => s == ':'))
            {
                return null;
            }

            string relativePath = inputLink.StartsWith('/') ? inputLink : '/' + inputLink;

            return new Uri(baseStartLink + relativePath);
        }
    }
}
