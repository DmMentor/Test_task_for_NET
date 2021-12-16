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
            services.AddScoped<WebsiteCrawler>();
            services.AddScoped<HtmlCrawler>();
            services.AddScoped<SitemapCrawler>();
            services.AddScoped<ParseDocumentHtml>();
            services.AddScoped<ParseDocumentSitemap>();
            services.AddScoped<Converter>();
            services.AddScoped<HttpService>();
            services.AddScoped<LinkHandling>();
            services.AddScoped<LinkRequest>();

            return services;
        }
    }
}
