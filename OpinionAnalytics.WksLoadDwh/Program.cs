using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Application.DTOs;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Application.Services;
using OpinionAnalytics.Persistence.Api;
using OpinionAnalytics.Persistence.Csv;
using OpinionAnalytics.Persistence.Csv.Base;
using OpinionAnalytics.Persistence.Db;
using OpinionAnalytics.Persistence.Db.Context;
using OpinionAnalytics.Persistence.Dw;
using OpinionAnalytics.Persistence.Dw.Context;

namespace OpinionAnalytics.WksLoadDwh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<Worker>();

            builder.Services.AddDbContext<OpinionAnalyticsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("OltpConnection")));

            builder.Services.AddDbContext<DwhContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DwConnection")));

            builder.Services.AddSingleton(typeof(IFileReader<>), typeof(CsvReader<>));
            builder.Services.AddSingleton<IFileReader<CsvClienteDto>, CsvReader<CsvClienteDto>>();
            builder.Services.AddSingleton<IFileReader<CsvProductoDto>, CsvReader<CsvProductoDto>>();
            builder.Services.AddSingleton<IFileReader<CsvFuenteDto>, CsvReader<CsvFuenteDto>>();

            builder.Services.AddSingleton<ISurveyRepository, CsvSurveyRepository>();
            builder.Services.AddScoped<IReviewRepository, DbReviewRepository>();
            builder.Services.AddHttpClient<ICommentRepository, ApiCommentRepository>();

            builder.Services.AddScoped<IDwhRepository, DwhRepository>();

            builder.Services.AddScoped<IDimHandlerService, DimHandlerService>();
            builder.Services.AddScoped<IFactHandlerService, FactHandlerService>();

            var host = builder.Build();
            host.Run();
        }
    }
}