using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Parse_site
{
    class ParseHtmlDocument : IParse
    {
        private Uri _uri;

        List<string> DownloadFile(Uri uri)
        {
            List<string> htmlDocument = new List<string>();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            HttpClient client = new HttpClient();

            HttpResponseMessage http = client.GetAsync(uri).GetAwaiter().GetResult();


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

        List<string> CustomParse(Uri uri)
        {
            List<string> urls = DownloadFile(uri);

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

        public ParseHtmlDocument(Uri uri)
        {
            _uri = uri;
        }

        public async Task<List<string>> ParseAsync()
        {
            string htmlDocument = await DownloadFile(_uri.AbsoluteUri);

            var list = CustomParse(_uri);

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
                if (url != _uri.AbsoluteUri)
                    await RecursiveParse(listUrls, url);
            }

            return listUrls;
        }

        private async Task RecursiveParse(List<string> list1, string _url)
        {
            string htmlDocument;

            try
            {
                htmlDocument = await DownloadFile(_url);
            }
            catch (WebException)
            {
                list1.Remove(_url);
                return;
            }

            var list = CustomParse(new Uri(_url));

            var listUrlsHtml = new List<string>();

            foreach (var matchUrl in list)
            {
                string url = LinkFormatting(matchUrl);

                if (url != null && !list1.Contains(url))
                {
                    listUrlsHtml.Add(url);
                    list1.Add(url);
                }
            }

            foreach (string url in listUrlsHtml)
            {
                if (url != _url)
                    await RecursiveParse(list1, url);
            }
        }

        private string LinkFormatting(string url)
        {

            if (url.Contains("mailto:") || url.Contains("skype:"))
            {
                return null;
            }
            else if (url.Length <= 1)
            {
                return _uri.Scheme + "://" + _uri.Host + '/';
            }
            else if (url.IndexOfAny(new char[] { '#', '?' }, 0, 2) > 0 || url.Contains(_uri.Scheme + "://" + _uri.Host + "/?"))
            {
                return _uri.Scheme + "://" + _uri.Host;
            }
            else if (url.Contains("http"))
            {
                var uri = new Uri(url);

                return uri.Host == _uri.Host ? url : null;
            }
            else
            {

                if (url.StartsWith('/'))
                {
                    return _uri.Scheme + "://" + _uri.Host + url;
                }
                else
                {
                    return _uri.Scheme + "://" + _uri.Host + '/' + url;
                }
            }

            if (!url.EndsWith('/'))
            {
                return url + '/';
            }

            return url;
        }

        public async Task<string> DownloadFile(string url)
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
