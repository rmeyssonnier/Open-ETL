﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PampaSoft.Data.Etl.Api.DbContext;

namespace PampaSoft.Data.Etl.Api.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("PampaSoft.Data.Etl.Api.DbContext.Pipeline", b =>
                {
                    b.Property<int>("PipelineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PipelineName")
                        .HasColumnType("TEXT");

                    b.Property<string>("PipelineProp")
                        .HasColumnType("TEXT");

                    b.HasKey("PipelineId");

                    b.ToTable("Pipelines");
                });
#pragma warning restore 612, 618
        }
    }
}