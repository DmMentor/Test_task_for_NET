using InterviewTask.Logic.Services;

namespace InterviewTask.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            LinksDisplay linksDisplay = new LinksDisplay();
            LinkHandling linkHandling = new LinkHandling();
            LinkRequest linkRequest = new LinkRequest(linkHandling);
            Converter converter = new Converter();
            ConsoleApp consoleApp = new ConsoleApp(linksDisplay, converter, linkRequest);

            consoleApp.Run();
        }
    }
}