using System.Collections.Generic;
using System.Threading.Tasks;

namespace Parse_site
{
    interface IParse
    {
        Task<List<(string, double)>> Parse();
    }
}
