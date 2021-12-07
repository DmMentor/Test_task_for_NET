using System.Collections.Generic;

namespace Parse_site
{
    interface IParseDocument
    {
        List<string> ParseDocument<T>(string inputLink, IDownloadDocument<T> download) where T : class;
    }
}
