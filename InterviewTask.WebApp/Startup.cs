using InterviewTask.CrawlerLogic.Extensions;
using InterviewTask.EntityFramework;
using InterviewTask.Logic.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InterviewTask.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddEfRepository<CrawlerContext>(o =>
             {
                 o.UseSqlServer(Configuration.GetConnectionString("Connection"));
             });
            services.AddCrawlerServices();
            services.AddLogicServices();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Result}/{action=GetTest}/{page?}");
            });
        }
    }
}
