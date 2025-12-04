using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.DTOs
{
    public class CsvFuenteDto
    {
        [Name("IdFuente")]
        public string IdFuente { get; set; }
        [Name("TipoFuente")]
        public string TipoFuente { get; set; }

    }
}
