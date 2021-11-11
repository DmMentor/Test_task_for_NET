using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse_site
{
    class ComparerListUrls : IComparer<(string url, double response)>
    {
        public int Compare((string url, double response) ob1, (string url, double response) ob2)
        {
            if (ob1.response > ob2.response)
                return 1;
            if (ob1.response < ob2.response)
                return -1;
            else
                return 0;
        }
    }
}
