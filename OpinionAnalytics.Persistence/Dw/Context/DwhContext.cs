using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Domain.Entities.Dwh.Dimensions;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
