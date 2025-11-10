using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Domain.Entities.Dw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Db.Context
{
    public class OpinionAnalyticsContext : DbContext
    {
        public OpinionAnalyticsContext(DbContextOptions<OpinionAnalyticsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reviews>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Reviews> Reviews { get; set; }
    }
}
