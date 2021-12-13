using InterviewTask.Logic.Services;

namespace InterviewTask.ConsoleApp
{
    internal class Program
    {
        static private void Main(string[] args)
        {
            LinksDisplay linksDisplay = new LinksDisplay();
            LinkHandling linkHandling = new LinkHandling();
            Converter converter = new Converter();
            ConsoleApp consoleApp = new ConsoleApp(linksDisplay, converter, linkHandling);

            consoleApp.Run();
        }
    }
}