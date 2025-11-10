using CsvHelper;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Csv.Base
{
    public class CsvReader<T> : IFileReader<T> where T : class
    {
        private readonly ILogger<CsvReader<T>> _logger;

        public CsvReader(ILogger<CsvReader<T>> logger)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<T>> ReadFileAsync(string filePath)
        {
            var data = new List<T>();
            _logger.LogInformation("Iniciando parseo de archivo: {FilePath}", filePath);

            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    await foreach (var record in csv.GetRecordsAsync<T>())
                    {
                        data.Add(record);
                    }
                }

                _logger.LogInformation("Parseo completado. {Count} registros encontrados en {FilePath}", data.Count, filePath);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al PARSEAR el archivo: {FilePath}", filePath);
                return Enumerable.Empty<T>();
            }
        }
    }
}
