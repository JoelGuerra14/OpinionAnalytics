using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.Services
{
    public class ETLService : IETLService
    {
        private readonly ISurveyRepository _surveyRepo;
        private readonly IReviewRepository _reviewRepo;
        private readonly ICommentRepository _commentRepo;
        private readonly IDataLoaderRepository _dataLoader;
        private readonly ILogger<ETLService> _logger;

        public ETLService(
            ISurveyRepository surveyRepo,
            IReviewRepository reviewRepo,
            ICommentRepository commentRepo,
            IDataLoaderRepository dataLoader,
            ILogger<ETLService> logger)
        {
            _surveyRepo = surveyRepo;
            _reviewRepo = reviewRepo;
            _commentRepo = commentRepo;
            _dataLoader = dataLoader;
            _logger = logger;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando proceso ETL (fase Extract)...");

            try
            {
                var surveys = await _surveyRepo.GetAllAsync();
                var reviews = await _reviewRepo.GetAllAsync();
                var comments = await _commentRepo.GetAllAsync();

                int surveyCount = surveys.Count();
                int reviewCount = reviews.Count();
                int commentCount = comments.Count();
                int totalCount = surveyCount + reviewCount + commentCount;

                _logger.LogInformation("Extracción completada: {SurveyCount} surveys, {ReviewCount} reviews, {CommentCount} comments.", surveyCount, reviewCount, commentCount);

                _logger.LogInformation("Iniciando carga de {TotalCount} registros en Staging...", totalCount);

                await _dataLoader.SaveSurveysAsync(surveys);
                await _dataLoader.SaveReviewsAsync(reviews);
                await _dataLoader.SaveCommentsAsync(comments);

                _logger.LogInformation("-------------------------------------------------");
                _logger.LogInformation("Proceso ETL completado con éxito.");
                _logger.LogInformation("Resumen de carga en Staging:");
                _logger.LogInformation("- Encuestas (Surveys) procesadas: {SurveyCount}", surveyCount);
                _logger.LogInformation("- Reseñas (Reviews) procesadas: {ReviewCount}", reviewCount);
                _logger.LogInformation("- Comentarios (Comments) procesados: {CommentCount}", commentCount);
                _logger.LogInformation("-> Total de registros cargados: {TotalCount}", totalCount);
                _logger.LogInformation("-------------------------------------------------");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico durante el proceso ETL.");
            }
        }
    }
}
