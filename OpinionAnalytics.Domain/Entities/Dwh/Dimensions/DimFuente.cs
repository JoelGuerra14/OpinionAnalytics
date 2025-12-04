using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dwh.Dimensions
{
    [Table("DimFuente", Schema = "Dimension")]
    public class DimFuente
    {
        [Key]
        public int Fuente_Key { get; set; }
        public string NombreCanal { get; set; }      // Ej: F001, Facebook
        public string NombreTipoFuente { get; set; } // Ej: Red Social, Web
    }
}
