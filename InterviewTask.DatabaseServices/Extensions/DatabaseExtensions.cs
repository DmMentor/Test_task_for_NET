using InterviewTask.Logic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.Logic.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            services.AddScoped<DatabaseOperation>();

            return services;
        }
    }
}
