using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Csv;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Csv
{
    public class CsvSurveyRepository : ISurveyRepository
    {
        private readonly IFileReader<Survey> _fileReader;
        private readonly string _filePath;
        private readonly ILogger<CsvSurveyRepository> _logger; 

        public CsvSurveyRepository(
            IConfiguration config,
            IFileReader<Survey> fileReader,
            ILogger<CsvSurveyRepository> logger)
        {
            _filePath = config["DataSources:Surveys:Path"];
            _fileReader = fileReader;
            _logger = logger;

            if (string.IsNullOrEmpty(_filePath))
            {
                _logger.LogWarning("SurveyRepository fue inicializado sin un filePath.");
            }
        }

        public async Task<IEnumerable<Survey>> GetAllAsync()
        {
            _logger.LogInformation("Iniciando extracción de Surveys...");

            if (string.IsNullOrEmpty(_filePath) || !File.Exists(_filePath))
            {
                _logger.LogError("La ruta del archivo no está configurada o el archivo no existe en: {FilePath}", _filePath);
                return Enumerable.Empty<Survey>();
            }

            _logger.LogInformation("Ruta de archivo validada. Delegando al IFileReader para parsear: {FilePath}", _filePath);

            var data = await _fileReader.ReadFileAsync(_filePath);
            _logger.LogInformation("Lectura completada. Total: {Count} registros.", data.Count());
            return data;
        }
    }
}
