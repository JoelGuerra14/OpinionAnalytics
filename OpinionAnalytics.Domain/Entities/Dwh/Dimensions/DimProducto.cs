using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dwh.Dimensions
{
    [Table("DimProducto", Schema = "Dimension")]
    public class DimProducto
    {
        [Key]
        public int Product_Key { get; set; }
        public string Product_Id { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
    }
}
