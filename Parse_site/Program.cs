using System;

namespace Parse_site
{
    class Program
    {
        static void Main(string[] args)
        {
            CrawlerBuild crawlerBuild = new CrawlerBuild();

            try
            {
                Console.Write("Input url website: ");
                string url = Console.ReadLine();
                //string url = "https://ukad-group.com";

                Crawler crawler = crawlerBuild.Build(url);

                Console.WriteLine("Starting parsing website....\n\n\n");
                crawler.Start();
            }
            catch (FormatException fEx)
            {
                Console.WriteLine(fEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}