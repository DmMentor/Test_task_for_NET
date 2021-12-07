using System;
using System.Collections.Generic;

namespace ParseDocument
{
    public interface IParseDocument
    {
        List<Uri> ParseDocument(string document);
    }
}
