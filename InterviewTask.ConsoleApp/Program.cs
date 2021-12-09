namespace InterviewTask.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            LinksDisplay linksDisplay = new LinksDisplay();
            ConsoleApp consoleApp = new ConsoleApp(linksDisplay);
            consoleApp.Run();
        }
    }
}