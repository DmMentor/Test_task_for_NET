using InterviewTask.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.Logic.Extensions
{
    public static class LogicExtensions
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            services.AddScoped<DatabaseOperation>();
            services.AddScoped<WebApplication>();
            services.AddScoped<LinkValidator>();

            return services;
        }
    }
}
