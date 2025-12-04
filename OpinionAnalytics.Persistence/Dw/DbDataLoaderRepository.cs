using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
using OpinionAnalytics.Persistence.Db.Context;
using OpinionAnalytics.Persistence.Dw.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Dw
{
    public class DbDataLoaderRepository : IDataLoaderRepository
    {
        private readonly DwhContext _context;
        private readonly ILogger<DbDataLoaderRepository> _logger;

        public DbDataLoaderRepository(DwhContext context, ILogger<DbDataLoaderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Provisional para guardar las tablas staging del fact
        public async Task SaveSurveysAsync(IEnumerable<Survey> surveys)
        {
            if (surveys == null || !surveys.Any())
            {
                _logger.LogWarning("No hay encuestas para guardar.");
                return;
            }

            _logger.LogInformation("Guardando {Count} encuestas en Stg_Surveys...", surveys.Count());

            foreach (var s in surveys)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    @"INSERT INTO Stg_Surveys 
                      (IdOpinion, IdCliente, IdProducto, Fecha, Comentario, Clasificacion, PuntajeSatisfaccion, Fuente)
                      VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})",
                    s.IdOpinion,
                    s.IdCliente,
                    s.IdProducto,
                    s.Fecha ?? (object)DBNull.Value,
                    s.Comentario,
                    s.Clasificacion,
                    s.PuntajeSatisfaccion ?? (object)DBNull.Value,
                    s.Fuente
                );
            }
        }

        public async Task SaveReviewsAsync(IEnumerable<Reviews> reviews)
        {
            _logger.LogInformation("Guardando {Count} reseñas en Stg_Reviews...", reviews.Count());

            foreach (var r in reviews)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Stg_Reviews (IdReview, IdCliente, IdProducto, Fecha, Comentario, Rating) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    r.IdReview, r.IdCliente, r.IdProducto, r.Fecha, r.Comentario, r.Rating
                );
            }
        }

        public async Task SaveCommentsAsync(IEnumerable<Comment> comments)
        {
            _logger.LogInformation("Guardando {Count} comentarios en Stg_Comments...", comments.Count());

            foreach (var c in comments)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO Stg_Comments (IdComment, IdCliente, IdProducto, Fuente, Comentario, Fecha) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    c.IdComment, c.IdCliente, c.IdProducto, c.Fuente, c.Comentario, c.Fecha
                );
            }
        }
    }
}
