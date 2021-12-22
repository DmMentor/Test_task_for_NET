using InterviewTask.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterviewTask.EntityFramework.EntityConfigurations
{
    internal class CrawlingResultConfiguration : IEntityTypeConfiguration<CrawlingResult>
    {
        public void Configure(EntityTypeBuilder<CrawlingResult> builder)
        {
            builder.Property(p => p.IsLinkFromHtml);
            builder.Property(p => p.IsLinkFromSitemap);
            builder.Property(p => p.Url).HasColumnName("Links");
        }
    }
}
