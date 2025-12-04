using OpinionAnalytics.Domain.Entities.Dwh.Dimensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.DTOs
{
    public class DimEntities
    {
        public List<DimCliente> Clientes { get; set; } = new();
        public List<DimProducto> Productos { get; set; } = new();
        public List<DimFuente> Fuentes { get; set; } = new();
        public List<DimClasificacion> Clasificaciones { get; set; } = new();
        public List<DimDate> Fechas { get; set; } = new();
    }
}
