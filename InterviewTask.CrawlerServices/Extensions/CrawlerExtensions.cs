using InterviewTask.CrawlerServices.Crawlers;
using InterviewTask.CrawlerServices.Parsers;
using InterviewTask.CrawlerServices.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.CrawlerServices.Extensions
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
