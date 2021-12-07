using System;

namespace Parse_site
{
    class Info : IComparable
    {
        public Uri _uri;
        public int _response;

        public Info(Uri uri, int response)
        {
            _uri = uri;
            _response = response;
        }

        public int CompareTo(object info)
        {
            if (info is Info i)
            {
                if (i._response < _response)
                {
                    return 1;
                }
                else if (i._response > _response)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
