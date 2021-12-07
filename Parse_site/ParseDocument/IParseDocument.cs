using System;
using System.Collections.Generic;

namespace Parse_site.ParseDocument
{
    interface IParseDocument
    {
        List<Uri> ParseDocument(string document);
    }
}
