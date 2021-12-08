using System;
using System.Diagnostics;
using System.Net;

namespace Parse_site
{
    class LinkRequest
    {
        public int SendRequest(Uri link)
        {
            try
            {
                var time = Stopwatch.StartNew();
                ((HttpWebRequest)WebRequest.Create(link)).GetResponse().Close();
                time.Stop();

                int response = time.Elapsed.Milliseconds;

                return response;
            }
            catch
            {
                return 0; //link don't work
            }
        }
    }
}
