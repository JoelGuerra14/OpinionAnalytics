using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Domain.Entities.Csv
{
    public class Survey
    {
        [Name("IdOpinion")]
        public string IdOpinion { get; set; }

        [Name("IdCliente")]
        public string IdCliente { get; set; }

        [Name("IdProducto")]
        public string IdProducto { get; set; }

        [Name("Fecha")]
        public DateTime? Fecha { get; set; }

        [Name("Comentario")]
        public string Comentario { get; set; }

        [Name("Clasificación")]
        public string Clasificacion { get; set; }

        [Name("PuntajeSatisfacción")]
        public int? PuntajeSatisfaccion { get; set; }

        [Name("Fuente")]
        public string Fuente { get; set; }

        public override string ToString()
        {
            return $"IdOpinion: {IdOpinion}, IdCliente: {IdCliente}, IdProducto: {IdProducto}, " +
                   $"Fecha: {Fecha?.ToString("yyyy-MM-dd") ?? "Sin fecha"}, " +
                   $"Comentario: {Comentario}, Clasificación: {Clasificacion}, " +
                   $"Puntaje: {PuntajeSatisfaccion?.ToString() ?? "N/A"}, Fuente: {Fuente}";
        }

    }
}
