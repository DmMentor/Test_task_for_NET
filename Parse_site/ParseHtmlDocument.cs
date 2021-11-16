using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace Parse_site
{
    class ParseHtmlDocument : IParse
    {
        private Uri _uri;

        public ParseHtmlDocument(Uri uri)
        {
            _uri = uri;
        }

        public async Task<List<string>> ParseAsync()
        {
            string htmlDocument;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_uri);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    htmlDocument = await streamReader.ReadToEndAsync();
                }
            }

            Regex reg = new Regex("<a href=\"(.*?)\"", RegexOptions.Singleline);
            Regex reg1 = new Regex("<a class=\".*?\" href=\"(.*?)\"", RegexOptions.Singleline);
            MatchCollection matchCollectionUrls = reg.Matches(htmlDocument);

            if (matchCollectionUrls.Count == 0)
                return null;

            var listUrlsHtml = new List<string>(matchCollectionUrls.Count);
            bool inAddlist = false;

            for (int i = 0; i < 2; i++)
            {
                foreach (Match matchUrl in matchCollectionUrls)
                {
                    string url = matchUrl.Groups[1].Value;

                    if (url.Contains("mailto:") || url.Contains("skype:"))
                    {
                        continue;
                    }
                    else if (url.Contains("http://") || url.Contains("https://"))
                    {
                        if (!(_uri.Host == new Uri(url).Host))
                        {
                            continue;
                        }

                        inAddlist = true;
                    }
                    else if (url.Length == 1)
                    {
                        url = Uri.UriSchemeHttps + "://" + _uri.Host;
                        inAddlist = true;
                    }
                    else if (url.IndexOf('#', 0, 2) != -1 || url.IndexOf('#', 0, 2) != -1)
                    {
                        url = Uri.UriSchemeHttps + "://" + _uri.Host;
                        inAddlist = true;
                    }
                    else
                    {
                        if (url[0] == '/')
                        {
                            url = Uri.UriSchemeHttps + "://" + _uri.Host + url;
                            inAddlist = true;
                        }
                        else
                        {
                            url = Uri.UriSchemeHttps + "://" + _uri.Host + '/' + url;
                            inAddlist = true;
                        }
                    }

                    if (!(url[url.Length - 1] == '/'))
                        url += '/';

                    if (!listUrlsHtml.Contains(url) && inAddlist)
                    {
                        listUrlsHtml.Add(url);
                    }

                    inAddlist = false;
                }

                matchCollectionUrls = reg1.Matches(htmlDocument);
            }

            var listUrls = new List<string>(listUrlsHtml);

            foreach (string url in listUrlsHtml)
            {
                if (url != _uri.AbsoluteUri)
                    await Par(listUrls, url);
            }

            return listUrls;
        }

        private async Task Par(List<string> list, string _url)
        {
            string htmlDocument;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    htmlDocument = await streamReader.ReadToEndAsync();
                }
            }

            Regex reg = new Regex("<a href=\"(.*?)\"", RegexOptions.Singleline);
            Regex reg1 = new Regex("<a class=\".*?\" href=\"(.*?)\"", RegexOptions.Singleline);
            MatchCollection matchCollectionUrls = reg.Matches(htmlDocument);

            var listUrlsHtml = new List<string>(matchCollectionUrls.Count);
            bool inAddlist = false;

            for (int i = 0; i < 2; i++)
            {
                foreach (Match matchUrl in matchCollectionUrls)
                {
                    string url = matchUrl.Groups[1].Value;

                    if (url.Contains("mailto:") || url.Contains("skype:"))
                    {
                        continue;
                    }
                     else if (url.Contains("http://") || url.Contains("https://"))
                    {
                        if (!(_uri.Host == new Uri(url).Host))
                        {
                            continue;
                        }

                        inAddlist = true;
                    }
                    else if (url.Length == 1)
                    {
                        url = Uri.UriSchemeHttps + "://" + _uri.Host;
                        inAddlist = true;
                    }
                    else if (url.IndexOf('#', 0, 2) != -1 || url.IndexOf('#', 0, 2) != -1)
                    {
                        url = Uri.UriSchemeHttps + "://" + _uri.Host;
                        inAddlist = true;
                    }
                    else
                    {
                        if (url[0] == '/')
                        {
                            url = Uri.UriSchemeHttps + "://" + _uri.Host + url;
                            inAddlist = true;
                        }
                        else
                        {
                            url = Uri.UriSchemeHttps + "://" + _uri.Host + '/' + url;
                            inAddlist = true;
                        }
                    }

                    if (!(url[url.Length - 1] == '/'))
                        url += '/';

                    if (!list.Contains(url) && inAddlist)
                    {
                        listUrlsHtml.Add(url);
                        list.Add(url);
                    }

                    inAddlist = false;
                }

                matchCollectionUrls = reg1.Matches(htmlDocument);
            }

            foreach (string url in listUrlsHtml)
            {
                await Par(list, url);
            }
        }

        private List<string> ConcatListUrls(List<string> list1, List<string> list2)
        {
            if (list2 != null)
                foreach (var url in list2)
                {
                    if (!list1.Contains(url))
                    {
                        list1.Add(url);
                    }
                }

            return list1;
        }

    }
}
