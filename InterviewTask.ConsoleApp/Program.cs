using InterviewTask.CrawlerLogic.Extensions;
using InterviewTask.Logic.Extensions;
using InterviewTask.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InterviewTask.ConsoleApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var consoleApp = CreateHostBuilder(args)
                             .Build()
                             .Services
                             .GetService<ConsoleApplication>();
            await consoleApp.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureServices((context, services) =>
               {
                   services.AddScoped<ConsoleApplication>()
                     .AddScoped<LinksDisplay>()
                     .AddEfRepository<CrawlerContext>(o =>
                     {
                         o.UseSqlServer(context.Configuration.GetConnectionString("Connection"));
                     })
                     .AddCrawlerServices()
                     .AddLogicServices();
               })
               .ConfigureLogging(options => options.SetMinimumLevel(LogLevel.Error));
    }
}