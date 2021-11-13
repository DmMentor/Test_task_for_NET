using System;
using System.Diagnostics;
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

        public async Task<List<(string, double)>> Parse()
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

            var listUrlsHtml = new List<(string url, double response)>(matchCollectionUrls.Count);
            var listUrls = new List<string>(matchCollectionUrls.Count);

            Stopwatch time = new Stopwatch();

            foreach (Match matchUrl in matchCollectionUrls)
            {
                string url;
                if (!matchUrl.Groups[1].Value.Contains("https://"))
                {
                    if (matchUrl.Groups[1].Value[0] == '/')
                    {
                        url = Uri.UriSchemeHttps + "://" + _uri.Host + matchUrl.Groups[1].Value;
                    }
                    else
                        url = Uri.UriSchemeHttps + "://" + _uri.Host + "/" + matchUrl.Groups[1].Value;
                }
                else
                    url = matchUrl.Groups[1].Value;

                if (!listUrls.Contains(url))
                {
                    try
                    {
                        time.Start();
                        ((HttpWebRequest)WebRequest.Create(url)).GetResponse().Close();
                        time.Stop();

                        listUrlsHtml.Add((url, time.Elapsed.TotalMilliseconds));
                        listUrls.Add(url);
                    }
                    catch (WebException)
                    {
                        listUrls.Add(url);

                        url += " --- not work";
                        listUrlsHtml.Add((url, -1));
                    }
                    finally
                    {
                        time.Reset();
                    }
                }
            }

            return listUrlsHtml;
        }
    }
}
