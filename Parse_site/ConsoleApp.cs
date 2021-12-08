using InterviewTask.Logic.Crawler;
using System;

namespace InterviewTask.ConsoleApp
{
    class ConsoleApp
    {
        private readonly LinksDisplay _linksDisplay;

        public ConsoleApp(LinksDisplay linksDisplay)
        {
            _linksDisplay = linksDisplay;
        }

        public void Run()
        {
            WebsiteCrawlerBuild crawlerBuild = new WebsiteCrawlerBuild();

            try
            {
                Console.Write("Input url website: ");
                // string url = Console.ReadLine();
                string link = "https://www.ukad-group.com";

                if (!Uri.TryCreate(link, UriKind.Absolute, out Uri linkUri))
                {
                    Console.WriteLine("Link is invalid");
                    return;
                }

                WebsiteCrawler crawler = crawlerBuild.Build();

                Console.WriteLine($"Starting parsing website....{Environment.NewLine}");

                var listAllLinks = crawler.Start(linkUri);

                _linksDisplay.DisplayResults(listAllLinks);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
