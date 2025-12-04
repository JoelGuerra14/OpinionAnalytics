using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.DTOs
{
    public class CsvClienteDto
    {
        [Name("IdCliente")]
        public string IdCliente { get; set; }
        [Name("Nombre")]
        public string Nombre { get; set; }
        [Name("Email")]
        public string Email { get; set; }
    }
}
