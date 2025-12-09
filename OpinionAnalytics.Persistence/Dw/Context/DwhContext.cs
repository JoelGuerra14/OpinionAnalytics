using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
using OpinionAnalytics.Domain.Entities.Dwh.Dimensions;
using OpinionAnalytics.Domain.Entities.Dwh.Facts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Dw.Context
{
    public class DwhContext : DbContext
    {
        public DwhContext(DbContextOptions<DwhContext> options) : base(options)
        {
        }
        public DbSet<DimCliente> DimClientes { get; set; }
        public DbSet<DimProducto> DimProductos { get; set; }
        public DbSet<DimFuente> DimFuentes { get; set; }
        public DbSet<DimClasificacion> DimClasificaciones { get; set; }
        public DbSet<DimDate> DimDates { get; set; }

        // Staging
        public DbSet<Survey> StgSurveys { get; set; }
        public DbSet<Reviews> StgReviews { get; set; }
        public DbSet<Comment> StgComments { get; set; }

        public DbSet<FactOpinion> FactOpiniones { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Survey>().ToTable("Stg_Surveys", "dbo");
            modelBuilder.Entity<Reviews>().ToTable("Stg_Reviews", "dbo");
            modelBuilder.Entity<Comment>().ToTable("Stg_Comments", "dbo");

            base.OnModelCreating(modelBuilder);
        }
    }
}
