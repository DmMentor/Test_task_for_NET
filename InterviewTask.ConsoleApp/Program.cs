using InterviewTask.Logic.Services;

namespace InterviewTask.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var linksDisplay = new LinksDisplay();
            var httpService = new HttpService();
            var linkHandling = new LinkHandling(httpService);

            var linkRequest = new LinkRequest(linkHandling);
            var converter = new Converter();
            var consoleApp = new ConsoleApp(linksDisplay, converter, linkRequest);

            consoleApp.Run();
        }
    }
}