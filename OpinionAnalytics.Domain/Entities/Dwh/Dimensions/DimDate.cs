using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dwh.Dimensions
{
    [Table("DimDate", Schema = "Dimension")]
    public class DimDate
    {
        [Key]
        public int Date_Key { get; set; }
        public DateTime FechaCompleta { get; set; }
        public int Anio { get; set; }
        public int Trimestre { get; set; }
        public int Mes { get; set; }
        public string NombreMes { get; set; }
        public int Dia { get; set; }
        public string DiaSemana { get; set; }
    }
}
