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
            MatchCollection matchCollectionUrls = reg.Matches(htmlDocument);

            if (matchCollectionUrls.Count == 0)
                return null;

            var listUrlsHtml = new List<string>(matchCollectionUrls.Count);

            foreach (Match matchUrl in matchCollectionUrls)
            {
                string url;

                if (!matchUrl.Groups[1].Value.Contains("mailto:") && !matchUrl.Groups[1].Value.Contains("skype:"))
                {
                    if (matchUrl.Groups[1].Value == "/" || matchUrl.Groups[1].Value == "#")
                    {
                        url = Uri.UriSchemeHttps + "://" + _uri.Host;
                    }
                    else if (!matchUrl.Groups[1].Value.Contains("https://") && !matchUrl.Groups[1].Value.Contains("http://"))
                    {
                        try
                        {
                            ((HttpWebRequest)WebRequest.Create(Uri.UriSchemeHttps + ":" + matchUrl.Groups[1].Value)).GetResponse().Close();
                            url = Uri.UriSchemeHttps + ":" + matchUrl.Groups[1].Value;
                        }
                        catch (Exception)
                        {
                            if (matchUrl.Groups[1].Value[0] == '/')
                            {
                                url = Uri.UriSchemeHttps + "://" + _uri.Host + matchUrl.Groups[1].Value;
                            }
                            else
                                url = Uri.UriSchemeHttps + "://" + _uri.Host + "/" + matchUrl.Groups[1].Value;
                        }
                    }
                    else
                    {
                        url = matchUrl.Groups[1].Value;
                    }

                    if (!listUrlsHtml.Contains(url))
                    {
                        listUrlsHtml.Add(url);
                    }
                }
            }

            return listUrlsHtml;
        }
    }
}
