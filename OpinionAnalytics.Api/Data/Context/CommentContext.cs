using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Domain.Entities.Dw;
using OpinionAnalytics.Persistence.Db.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Api.Data.Context
{
    public class CommentContext :DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Comment> Comments { get; set; }
    }
}
