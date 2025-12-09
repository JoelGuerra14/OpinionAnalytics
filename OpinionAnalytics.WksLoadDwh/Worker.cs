using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;

namespace OpinionAnalytics.WksLoadDwh
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker iniciado");

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {

                    var surveyRepo = scope.ServiceProvider.GetRequiredService<ISurveyRepository>();
                    var reviewRepo = scope.ServiceProvider.GetRequiredService<IReviewRepository>();
                    var commentRepo = scope.ServiceProvider.GetRequiredService<ICommentRepository>();

                    var dwhRepo = scope.ServiceProvider.GetRequiredService<IDwhRepository>();
                    var dimHandler = scope.ServiceProvider.GetRequiredService<IDimHandlerService>();
                    var factHandler = scope.ServiceProvider.GetRequiredService<IFactHandlerService>();


                    _logger.LogInformation("1. Extrayendo datos de fuentes...");
                    var surveys = await surveyRepo.GetAllAsync();
                    var reviews = await reviewRepo.GetAllAsync();
                    var comments = await commentRepo.GetAllAsync();

                    _logger.LogInformation("2. Cargando Staging...");
                    await dwhRepo.LoadStagingAsync(surveys, reviews, comments);

                    _logger.LogInformation("3. Procesando Dimensiones...");
                    await dimHandler.ProcessDimensionsAsync();

                    _logger.LogInformation("4. Procesando Facts...");
                    await factHandler.ProcessFactsAsync();

                    _logger.LogInformation("PROCESO ETL COMPLETO EXITOSAMENTE.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error crítico en el pipeline ETL.");
                }
            }
        }
    }
}