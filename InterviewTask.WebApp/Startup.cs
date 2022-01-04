using InterviewTask.CrawlerLogic.Extensions;
using InterviewTask.Logic.Extensions;
using InterviewTask.EntityFramework;
using InterviewTask.WebApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace InterviewTask.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddEfRepository<CrawlerContext>(o =>
             {
                 o.UseSqlServer(Configuration.GetConnectionString("Connection"));
             });
            services.AddCrawlerServices();
            services.AddDatabaseServices();
            services.AddScoped<Crawler>();
            services.AddScoped<LinkValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Database}/{action=GetTest}/{id?}");
            });
        }
    }
}
