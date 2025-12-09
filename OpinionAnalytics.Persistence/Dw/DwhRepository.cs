using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.DTOs;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
using OpinionAnalytics.Domain.Entities.Dwh.Dimensions;
using OpinionAnalytics.Domain.Entities.Dwh.Facts;
using OpinionAnalytics.Persistence.Dw.Context;

namespace OpinionAnalytics.Persistence.Dw
{
    public class DwhRepository : IDwhRepository
    {
        private readonly DwhContext _context;
        private readonly ILogger<DwhRepository> _logger;

        public DwhRepository(DwhContext context, ILogger<DwhRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        //Carga de Staging
        public async Task LoadStagingAsync(IEnumerable<Survey> surveys, IEnumerable<Reviews> reviews, IEnumerable<Comment> comments)
        {
            _logger.LogInformation("--- Iniciando Carga a Staging ---");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Stg_Surveys");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Stg_Reviews");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM Stg_Comments");

                if (surveys.Any()) await _context.StgSurveys.AddRangeAsync(surveys);
                if (reviews.Any()) await _context.StgReviews.AddRangeAsync(reviews);
                if (comments.Any()) await _context.StgComments.AddRangeAsync(comments);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                _logger.LogInformation("Datos cargados en Staging correctamente.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        //Funciones para obtener datos de tablas staging y dimensiones
        public IQueryable<Survey> GetSurveysQuery() => _context.StgSurveys.AsNoTracking();
        public IQueryable<Reviews> GetReviewsQuery() => _context.StgReviews.AsNoTracking();
        public IQueryable<Comment> GetCommentsQuery() => _context.StgComments.AsNoTracking();

        public IQueryable<DimCliente> GetDimClientesQuery() => _context.DimClientes.AsNoTracking();
        public IQueryable<DimProducto> GetDimProductosQuery() => _context.DimProductos.AsNoTracking();
        public IQueryable<DimDate> GetDimDatesQuery() => _context.DimDates.AsNoTracking();

        public async Task<List<DimFuente>> GetDimFuentesListAsync() => await _context.DimFuentes.AsNoTracking().ToListAsync();
        public async Task<List<DimClasificacion>> GetDimClasificacionesListAsync() => await _context.DimClasificaciones.AsNoTracking().ToListAsync();

        //Cargar Fact
        public async Task SaveFactsAsync(List<FactOpinion> facts)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Fact].[FactOpiniones]");
                await _context.FactOpiniones.AddRangeAsync(facts);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch { await transaction.RollbackAsync(); throw; }
        }

        //Cargar Dimensiones
        public async Task LoadDimDataAsync(DimEntities entidades)
        {
            _logger.LogInformation("Guardando Dimensiones en Base de Datos");

            await CleanDimensions();

            await _context.DimClientes.AddRangeAsync(entidades.Clientes);
            await _context.DimProductos.AddRangeAsync(entidades.Productos);
            await _context.DimFuentes.AddRangeAsync(entidades.Fuentes);
            await _context.DimClasificaciones.AddRangeAsync(entidades.Clasificaciones);
            await _context.DimDates.AddRangeAsync(entidades.Fechas);

            await _context.SaveChangesAsync();

            _logger.LogInformation("Carga de dimensiones finalizada en BD");
        }

        private async Task CleanDimensions()
        {
            _logger.LogInformation("Iniciando limpieza de tablas Staging y DWH");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Limpiando Fact Table para liberar restricciones...");
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Fact].[FactOpiniones]");

                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[DimCliente]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[DimCliente]', RESEED, 0)");

                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[DimProducto]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[DimProducto]', RESEED, 0)");

                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[DimFuente]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[DimFuente]', RESEED, 0)");

                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[DimClasificacion]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[DimClasificacion]', RESEED, 0)");

                await _context.Database.ExecuteSqlRawAsync("DELETE FROM [Dimension].[DimDate]");
                await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('[Dimension].[DimDate]', RESEED, 0)");

                await transaction.CommitAsync();
                _logger.LogInformation("Tablas limpiadas correctamente");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error crítico al limpiar las tablas");
                throw;
            }
        }
    }
}