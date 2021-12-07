using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Linq;

namespace Parse_site
{
    class ParseHtmlDocument : IParse
    {
        private readonly Uri baseLink;

        List<string> DownloadFile(string inputLink)
        {
            List<string> htmlDocument = new List<string>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inputLink);

            HttpClient client = new HttpClient();

            HttpResponseMessage http = client.GetAsync(inputLink).GetAwaiter().GetResult();


            //string s = http.Content.ReadAsStringAsync().Result;

            var stream = new StreamReader(http.Content.ReadAsStreamAsync().Result);

            while (true)
            {
                string s1 = stream.ReadLine();

                if (s1 != null)
                {
                    htmlDocument.Add(s1);
                }
                else
                {
                    break;
                }
            }

            //Console.WriteLine(htmlDocument);

            return htmlDocument;
        }

        List<string> CustomParse(string inputLink)
        {
            List<string> urls = DownloadFile(inputLink);

            List<string> res1 = new List<string>();

            List<string> res2 = new List<string>();

            List<string> res3 = new List<string>();


            foreach (var item in urls)
            {
                if (item.Contains("<a"))
                {
                    res1.Add(item);
                }
            }

            string conditionalLink = "href=\"";
            foreach (var item in res1)
            {
                int startIndex = item.IndexOf(conditionalLink);

                if (startIndex > -1)
                {
                    startIndex += conditionalLink.Length;
                    int length = item.IndexOf("\"");

                    res2.Add(item.Substring(startIndex));
                }
            }



            foreach (var item in res2)
            {
                int length = item.IndexOf("\"");

                res3.Add(item.Substring(0, length));
            }


            return res3;
        }

        public ParseHtmlDocument(Uri link)
        {
            baseLink = link;
        }

        public List<string> ParseAsync()
        {
            var list = CustomParse(baseLink.AbsoluteUri);

            var listUrlsHtml = new List<string>();

            foreach (var matchUrl in list)
            {
                string url = LinkFormatting(matchUrl);

                if (url != null && !listUrlsHtml.Contains(url))
                {
                    listUrlsHtml.Add(url);
                }
            }

            var listUrls = new List<string>(listUrlsHtml);

            foreach (string url in listUrlsHtml)
            {
                if (url != baseLink.AbsoluteUri)
                    RecursiveParse(listUrls, url);
            }

            return listUrls;
        }

        private void RecursiveParse(List<string> listOldLinks, string inputLink)
        {
            var listLinksFromHtml = CustomParse(inputLink);

            var listNewLinks = new List<string>();

            foreach (var link in listLinksFromHtml)
            {
                string formattedLink = LinkFormatting(link);

                if (formattedLink != null && !listOldLinks.Contains(formattedLink))
                {
                    listNewLinks.Add(formattedLink);
                    listOldLinks.Add(formattedLink);
                }
            }

            if (listNewLinks.Count > 0)
            {
                foreach (string newLink in listNewLinks)
                {
                    if (newLink != inputLink)
                    {
                        Console.WriteLine(newLink);
                        RecursiveParse(listOldLinks, newLink);
                    }
                }
            }
        }

        private string LinkFormatting(string inputLink)
        {
            string formattedLink = string.Empty;
            string baseStartLink = baseLink.Scheme + "://" + baseLink.Host;

            if (inputLink.Length <= 1)
            {
                formattedLink = baseLink.Scheme + "://" + baseLink.Host + '/';
            }
            else if (inputLink.Contains("/?") || inputLink.Contains("#"))
            {
                formattedLink = baseStartLink;
            }
            else if (inputLink.Contains("http"))
            {
                var uri = new Uri(inputLink);

                if (uri.Host == baseLink.Host)
                {
                    formattedLink = inputLink;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (inputLink.Where(s => s == ':').Count() > 0)
                {
                    return null;
                }

                string s = inputLink.StartsWith('/') ? inputLink : '/' + inputLink;

                formattedLink = baseStartLink + s;
            }

            return formattedLink.EndsWith('/') ? formattedLink : formattedLink + '/';
        }

        public async Task<string> DownloadFileAsync(string url)
        {
            string htmlDocument;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    htmlDocument = await streamReader.ReadToEndAsync();
                }
            }

            return htmlDocument;
        }
    }
}
