using InterviewTask.Logic.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InterviewTask.ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<ConsoleHostedService>();
                    services.AddScoped<ConsoleApp>();
                    services.AddScoped<LinksDisplay>();
                    services.AddInterviewTaskLogicServices();
                })
                .ConfigureLogging(options => options.ClearProviders());
    }
}