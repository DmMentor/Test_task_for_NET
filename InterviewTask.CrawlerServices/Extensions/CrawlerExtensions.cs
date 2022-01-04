using InterviewTask.CrawlerLogic.Crawlers;
using InterviewTask.CrawlerLogic.Parsers;
using InterviewTask.CrawlerLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.CrawlerLogic.Extensions
{
    public static class CrawlerExtensions
    {
        public static IServiceCollection AddCrawlerServices(this IServiceCollection services)
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
