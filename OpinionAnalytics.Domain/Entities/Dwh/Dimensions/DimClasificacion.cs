using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dwh.Dimensions
{
    [Table("DimClasificacion", Schema = "Dimension")]
    public class DimClasificacion
    {
        [Key]
        public int Clasificacion_Key { get; set; }
        public string ClasificacionNombre { get; set; }
        public int ClasificacionValor { get; set; }   
    }
}
