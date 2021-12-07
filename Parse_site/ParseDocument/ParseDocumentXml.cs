using System.Collections.Generic;
using System.Xml;
using Parse_site.Download;

namespace Parse_site.ParseDocument
{
    class ParseDocumentXml : IParseDocument
    {
        public List<string> ParseDocument<T>(string inputLink, IDownloadDocument<T> download) where T : class
        {
            XmlNodeList xmlNodeList = ((DownloadDocumentXml)download).DownloadDocument(inputLink);

            if (xmlNodeList == null)
                return null;

            var listUrlsSitemap = new List<string>(xmlNodeList.Count);

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (!listUrlsSitemap.Contains(xmlNode.InnerText))
                {
                    listUrlsSitemap.Add(xmlNode.InnerText);
                }
            }

            return listUrlsSitemap;
        }
    }
}
