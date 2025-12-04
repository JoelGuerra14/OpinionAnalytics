using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.DTOs;
using OpinionAnalytics.Application.Repositories;
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