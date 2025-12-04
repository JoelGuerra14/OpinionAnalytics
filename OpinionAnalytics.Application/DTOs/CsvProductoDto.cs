using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.DTOs
{
    public class CsvProductoDto
    {
        [Name("IdProducto")]
        public string IdProducto { get; set; }
        [Name("Nombre")]
        public string Nombre { get; set; }
        [Name("Categoría")]
        public string Categoria { get; set; }
    }
}
