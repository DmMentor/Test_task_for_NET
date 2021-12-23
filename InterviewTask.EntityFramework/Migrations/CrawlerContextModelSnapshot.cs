﻿// <auto-generated />
using System;
using InterviewTask.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InterviewTask.EntityFramework.Migrations
{
    [DbContext(typeof(CrawlerContext))]
    partial class CrawlerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InterviewTask.EntityFramework.Entities.CrawlingResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsLinkFromHtml")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLinkFromSitemap")
                        .HasColumnType("bit");

                    b.Property<int>("ResponseTime")
                        .HasColumnType("int");

                    b.Property<int>("TestId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("InterviewTask.EntityFramework.Entities.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BaseUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ParsingDate")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(0)
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("InterviewTask.EntityFramework.Entities.CrawlingResult", b =>
                {
                    b.HasOne("InterviewTask.EntityFramework.Entities.Test", "Test")
                        .WithMany("Links")
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");
                });

            modelBuilder.Entity("InterviewTask.EntityFramework.Entities.Test", b =>
                {
                    b.Navigation("Links");
                });
#pragma warning restore 612, 618
        }
    }
}
