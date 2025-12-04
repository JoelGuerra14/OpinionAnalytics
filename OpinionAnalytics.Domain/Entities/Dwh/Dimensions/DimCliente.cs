using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dwh.Dimensions
{
    [Table("DimCliente", Schema = "Dimension")]
    public class DimCliente
    {
        [Key]
        public int Cliente_Key { get; set; }
        public string Cliente_Id { get; set; }
        public string Nombre { get; set; }
        public string? Pais { get; set; }
        public int? Edad { get; set; }
        public string? TipoCliente { get; set; }
    }
}
