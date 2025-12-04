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
            _logger.LogInformation("?? Worker iniciado.");

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    //Carga de Dimensiones
                    var dimHandler = scope.ServiceProvider.GetRequiredService<IDimHandlerService>();
                    await dimHandler.ProcessDimensionsAsync();


                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el proceso del Worker");
                }
            }

            // Carga de tablas staging
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var etl = scope.ServiceProvider.GetRequiredService<IETLService>();
            //    await etl.RunAsync(stoppingToken);
            //}

            _logger.LogInformation("?? ETL completado. Worker detenido.");
        }
    }
}
