using InterviewTask.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterviewTask.EntityFramework.EntityConfigurations
{
    internal class CrawlingResultConfiguration : IEntityTypeConfiguration<CrawlingResult>
    {
        public void Configure(EntityTypeBuilder<CrawlingResult> builder)
        {
            builder.Property(p => p.Url)
                   .HasMaxLength(2000);
            builder.HasOne(p => p.Test)
                   .WithMany(p => p.Links)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(true);
        }
    }
}
