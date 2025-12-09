using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Dwh.Facts;

namespace OpinionAnalytics.Application.Services
{
    public class FactHandlerService : IFactHandlerService
    {
        private readonly IDwhRepository _dwhRepo;
        private readonly ILogger<FactHandlerService> _logger;

        public FactHandlerService(IDwhRepository dwhRepo, ILogger<FactHandlerService> logger)
        {
            _dwhRepo = dwhRepo;
            _logger = logger;
        }

        public async Task ProcessFactsAsync()
        {
            _logger.LogInformation("Iniciando lógica de transformación en el Service...");

            var fuentes = await _dwhRepo.GetDimFuentesListAsync();
            var clasificaciones = await _dwhRepo.GetDimClasificacionesListAsync();

            int BuscarFuente(string nombre)
            {
                var match = fuentes.FirstOrDefault(f =>
                    f.NombreCanal.Trim().Equals(nombre, StringComparison.OrdinalIgnoreCase));
                return match?.Fuente_Key ?? 1;
            }

            int BuscarClasif(string nombre)
            {
                if (string.IsNullOrWhiteSpace(nombre)) nombre = "Neutra";

                var match = clasificaciones.FirstOrDefault(c =>
                    c.ClasificacionNombre.Trim().Equals(nombre.Trim(), StringComparison.OrdinalIgnoreCase));
                return match?.Clasificacion_Key ?? 1;
            }

            
            var keyFuenteEncuesta = BuscarFuente("EncuestaInterna");
            var keyFuenteWeb = BuscarFuente("Web");

            var keyClasifPositiva = BuscarClasif("Positiva");
            var keyClasifNegativa = BuscarClasif("Negativa");
            var keyClasifNeutra = BuscarClasif("Neutra");

            var listaFinalFacts = new List<FactOpinion>();

            //Procesar surveys
            var querySurveys = from s in _dwhRepo.GetSurveysQuery()
                               join c in _dwhRepo.GetDimClientesQuery() on s.IdCliente equals c.Cliente_Id
                               join p in _dwhRepo.GetDimProductosQuery() on s.IdProducto equals p.Product_Id
                               join d in _dwhRepo.GetDimDatesQuery() on s.Fecha equals d.FechaCompleta
                               select new
                               {
                                   s,
                                   c,
                                   p,
                                   d 
                               };

            foreach (var item in await querySurveys.ToListAsync())
            {
                listaFinalFacts.Add(new FactOpinion
                {
                    Cliente_Key = item.c.Cliente_Key,
                    Product_Key = item.p.Product_Key,
                    Date_Key = item.d.Date_Key,
                    Fuente_Key = keyFuenteEncuesta, 
                    Clasificacion_Key = BuscarClasif(item.s.Clasificacion),

                    PuntajeSatisfaccion = item.s.PuntajeSatisfaccion ?? 0,
                    IdFuenteOriginal = item.s.IdOpinion
                });
            }

            //Procesar reviews
            var queryReviews = from r in _dwhRepo.GetReviewsQuery()
                               join c in _dwhRepo.GetDimClientesQuery() on r.IdCliente equals c.Cliente_Id
                               join p in _dwhRepo.GetDimProductosQuery() on r.IdProducto equals p.Product_Id
                               join d in _dwhRepo.GetDimDatesQuery() on r.Fecha equals d.FechaCompleta
                               select new { r, c, p, d };

            foreach (var item in await queryReviews.ToListAsync())
            {
                int clasifKey = (item.r.Rating >= 4) ? keyClasifPositiva :
                                (item.r.Rating <= 2) ? keyClasifNegativa : keyClasifNeutra;

                listaFinalFacts.Add(new FactOpinion
                {
                    Cliente_Key = item.c.Cliente_Key,
                    Product_Key = item.p.Product_Key,
                    Date_Key = item.d.Date_Key,
                    Fuente_Key = keyFuenteWeb, 
                    Clasificacion_Key = clasifKey,
                    PuntajeSatisfaccion = item.r.Rating,
                    IdFuenteOriginal = item.r.IdReview
                });
            }

            //Procesar comments
            var queryComments = from cm in _dwhRepo.GetCommentsQuery()
                                join c in _dwhRepo.GetDimClientesQuery() on cm.IdCliente equals c.Cliente_Id
                                join p in _dwhRepo.GetDimProductosQuery() on cm.IdProducto equals p.Product_Id
                                join d in _dwhRepo.GetDimDatesQuery() on cm.Fecha equals d.FechaCompleta
                                select new { cm, c, p, d };

            foreach (var item in await queryComments.ToListAsync())
            {
                
                int fuenteKey = BuscarFuente(item.cm.Fuente);

                listaFinalFacts.Add(new FactOpinion
                {
                    Cliente_Key = item.c.Cliente_Key,
                    Product_Key = item.p.Product_Key,
                    Date_Key = item.d.Date_Key,
                    Fuente_Key = fuenteKey,
                    Clasificacion_Key = keyClasifNeutra, 
                    PuntajeSatisfaccion = null,
                    IdFuenteOriginal = item.cm.IdComment
                });
            }

            int contadorId = 1;
            foreach (var fact in listaFinalFacts)
            {
                fact.Opinion_Id = contadorId++;
            }

            if (listaFinalFacts.Any())
            {
                await _dwhRepo.SaveFactsAsync(listaFinalFacts);
                _logger.LogInformation("Carga de Facts finalizada. Total registros: {Count}", listaFinalFacts.Count);
            }
            else
            {
                _logger.LogWarning("No se generaron Facts.");
            }
        }
    }
}