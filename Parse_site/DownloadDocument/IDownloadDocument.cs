namespace Parse_site
{
    interface IDownloadDocument<T> where T : class
    {
        T DownloadDocument(string inputLink);
    }
}
