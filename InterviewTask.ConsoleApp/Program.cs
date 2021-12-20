using InterviewTask.Logic.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection()
                    .AddScoped<ConsoleApp>()
                    .AddScoped<LinksDisplay>()
                    .AddInterviewTaskLogicServices();

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var consoleApp = serviceProvider.GetService<ConsoleApp>();
                consoleApp.Run();
            }
        }
    }
}