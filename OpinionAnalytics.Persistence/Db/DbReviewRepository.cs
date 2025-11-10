using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Dw;
using OpinionAnalytics.Persistence.Db.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Db
{
    public class DbReviewRepository : IReviewRepository
    {
        private readonly OpinionAnalyticsContext _context;
        private readonly ILogger<DbReviewRepository> _logger;

        public DbReviewRepository(OpinionAnalyticsContext context, ILogger<DbReviewRepository> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<IEnumerable<Reviews>> GetAllAsync()
        {
            _logger.LogInformation("Iniciando extracción de reseñas desde la base de datos...");

            try
            {
                var reviews = await _context.Reviews.AsNoTracking().ToListAsync();
                _logger.LogInformation("{Count} reseñas extraídas correctamente.", reviews.Count);
                return reviews;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar extraer las reseñas desde la base de datos.");
                return Enumerable.Empty<Reviews>();
            }
        }
    }
}
