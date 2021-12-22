using InterviewTask.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InterviewTask.EntityFramework.EntityConfigurations
{
    internal class TestConfiguration : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> builder)
        {
            builder.Property(p => p.ParsingDate)
                   .HasPrecision(0)
                   .ValueGeneratedOnAdd()
                   .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
