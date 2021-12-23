using InterviewTask.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace InterviewTask.EntityFramework
{
    public class CrawlerContext : DbContext, IEfRepositoryDbContext
    {
        public DbSet<CrawlingResult> Links { get; set; }
        public DbSet<Test> Tests { get; set; }

        public CrawlerContext(DbContextOptions<CrawlerContext> dbContextOptions)
            : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
