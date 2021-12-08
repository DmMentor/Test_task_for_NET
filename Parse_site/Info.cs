using System;

namespace Parse_site
{
    class Info : IComparable
    {
        public Uri uri { get; set; }
        public int response { get; set; }

        public Info(Uri uri, int response)
        {
            this.uri = uri;
            this.response = response;
        }

        public int CompareTo(object info)
        {
            if (info is Info i)
            {
                if (i.response < response)
                {
                    return 1;
                }
                else if (i.response > response)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
