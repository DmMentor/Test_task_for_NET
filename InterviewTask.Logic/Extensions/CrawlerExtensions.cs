using InterviewTask.LogicCrawler.Crawlers;
using InterviewTask.LogicCrawler.Parsers;
using InterviewTask.LogicCrawler.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.LogicCrawler.Extensions
{
    public static class CrawlerExtensions
    {
        public static IServiceCollection AddLogicCrawlerServices(this IServiceCollection services)
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
