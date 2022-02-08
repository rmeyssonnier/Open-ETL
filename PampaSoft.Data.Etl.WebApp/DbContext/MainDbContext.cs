using System;
using Microsoft.EntityFrameworkCore;

namespace PampaSoft.Data.Etl.Api.DbContext
{
    public class MainDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<Pipeline> Pipelines { get; set; }
        public string DbPath { get; }

        public MainDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "maincontext.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");
    }

    public class Pipeline
    {
        public int PipelineId { get; set; }
        public string PipelineName { get; set; }
        public string PipelineProp { get; set; }
    }
}