using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.DTOs;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Dwh.Dimensions;
using System.Globalization;

namespace OpinionAnalytics.Application.Services
{
    public class DimHandlerService : IDimHandlerService
    {
        private readonly IFileReader<CsvClienteDto> _clienteReader;
        private readonly IFileReader<CsvProductoDto> _productoReader;
        private readonly IDwhRepository _dwhRepository;
        private readonly IConfiguration _config;
        private readonly ILogger<DimHandlerService> _logger;

        public DimHandlerService(
            IFileReader<CsvClienteDto> clienteReader,
            IFileReader<CsvProductoDto> productoReader,
            IDwhRepository dwhRepository,
            IConfiguration config,
            ILogger<DimHandlerService> logger)
        {
            _clienteReader = clienteReader;
            _productoReader = productoReader;
            _dwhRepository = dwhRepository;
            _config = config;
            _logger = logger;
        }

        public async Task ProcessDimensionsAsync()
        {
            _logger.LogInformation("Iniciando Transformación de Dimensiones...");

            string pathClientes = _config["DataSources:Dimensions:ClientesPath"];
            string pathProductos = _config["DataSources:Dimensions:ProductosPath"];

            var sourceClientes = await _clienteReader.ReadFileAsync(pathClientes);
            var sourceProductos = await _productoReader.ReadFileAsync(pathProductos);

            var entidades = new DimEntities();

            // Transformar clientes
            entidades.Clientes = sourceClientes.Select(c => new DimCliente
            {
                Cliente_Id = c.IdCliente,
                Nombre = c.Nombre,
                Pais = "N/A",
                Edad = 0,
                TipoCliente = "Regular"
            }).ToList();

            // Transformar productos
            entidades.Productos = sourceProductos.Select(p => new DimProducto
            {
                Product_Id = p.IdProducto,
                Nombre = p.Nombre,
                Categoria = p.Categoria
            }).ToList();

            // Transformar fuentes
            entidades.Fuentes = GenerarFuentes();

            // Transformar clasificaciones
            entidades.Clasificaciones = new List<DimClasificacion>
            {
                new DimClasificacion { ClasificacionNombre = "Positiva", ClasificacionValor = 1 },
                new DimClasificacion { ClasificacionNombre = "Negativa", ClasificacionValor = -1 },
                new DimClasificacion { ClasificacionNombre = "Neutra", ClasificacionValor = 0 }
            };

            // Generar fechas
            entidades.Fechas = GenerarFechas(new DateTime(2024, 1, 1), new DateTime(2025, 12, 31));

            // Carga final
            await _dwhRepository.LoadDimDataAsync(entidades);
        }

        // Generar Clasificaciones
        private List<DimFuente> GenerarFuentes()
        {
            return new List<DimFuente>
            {
                // Para surveys_part1.csv
                new DimFuente { NombreCanal = "EncuestaInterna", NombreTipoFuente = "CSV" },
                
                // Para web_reviews.csv (Database)
                new DimFuente { NombreCanal = "Web", NombreTipoFuente = "Base de datos" },
                
                // Para social_comments.csv (API) 
                new DimFuente { NombreCanal = "Twitter", NombreTipoFuente = "API REST" },
                new DimFuente { NombreCanal = "Facebook", NombreTipoFuente = "API REST" },
                new DimFuente { NombreCanal = "Instagram", NombreTipoFuente = "API REST" }
            };
        }

        // Generar DimFecha
        private List<DimDate> GenerarFechas(DateTime start, DateTime end)
        {
            var dates = new List<DimDate>();
            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                dates.Add(new DimDate
                {
                    FechaCompleta = dt,
                    Anio = dt.Year,
                    Mes = dt.Month,
                    Dia = dt.Day,
                    NombreMes = dt.ToString("MMMM", new CultureInfo("es-ES")),
                    DiaSemana = dt.ToString("dddd", new CultureInfo("es-ES")),
                    Trimestre = (dt.Month - 1) / 3 + 1
                });
            }
            return dates;
        }
    }
}