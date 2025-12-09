using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Dwh.Facts
{
    [Table("FactOpiniones", Schema = "Fact")]
    public class FactOpinion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int Opinion_Id { get; set; }
        public int Date_Key { get; set; }
        public int Product_Key { get; set; }
        public int Cliente_Key { get; set; }
        public int Fuente_Key { get; set; }
        public int Clasificacion_Key { get; set; }
        public int? PuntajeSatisfaccion { get; set; }
        public string? IdFuenteOriginal { get; set; }
    }
}
