using InterviewTask.LogicDatabase.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.LogicDatabase.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddLogicDatabaseServices(this IServiceCollection services)
        {
            services.AddScoped<DatabaseOperation>();

            return services;
        }
    }
}
