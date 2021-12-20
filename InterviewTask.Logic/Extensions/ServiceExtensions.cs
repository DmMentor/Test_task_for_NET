using InterviewTask.Logic.Crawlers;
using InterviewTask.Logic.Parsers;
using InterviewTask.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.Logic.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInterviewTaskLogicServices(this IServiceCollection services)
        {
            services.AddScoped<WebsiteCrawler>()
                    .AddScoped<HtmlCrawler>()
                    .AddScoped<SitemapCrawler>()
                    .AddScoped<ParseDocumentHtml>()
                    .AddScoped<ParseDocumentSitemap>()
                    .AddScoped<Converter>()
                    .AddScoped<HttpService>()
                    .AddScoped<LinkHandling>()
                    .AddScoped<LinkRequest>();

            return services;
        }
    }
}
