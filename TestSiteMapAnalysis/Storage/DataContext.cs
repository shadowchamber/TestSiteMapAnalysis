using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TestSiteMapAnalysis.Models;

namespace TestSiteMapAnalysis.Storage
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection") { }

        public DbSet<SitemapResult> SitemapResults { get; set; }
        public DbSet<SitemapItemResult> SitemapItemResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SitemapResult>()
                .HasMany(s => s.ItemResults)
                .WithRequired(e => e.SitemapResult)
                .HasForeignKey(e => e.SitemapResultRefRecGuid);
        }
    }
}
