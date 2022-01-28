using InterviewTask.Logic.Services;
using InterviewTask.Logic.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.Logic.Extensions
{
    public static class LogicExtensions
    {
        public static IServiceCollection AddLogicServices(this IServiceCollection services)
        {
            services.AddScoped<DatabaseService>();
            services.AddScoped<WebApplication>();
            services.AddScoped<LinkValidator>();

            return services;
        }
    }
}
