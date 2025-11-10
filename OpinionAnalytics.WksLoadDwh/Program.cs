using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Application.Services;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
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

            builder.Services.AddDbContext<DwDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DwConnection")));


            builder.Services.AddSingleton(typeof(IFileReader<>), typeof(CsvReader<>));

            builder.Services.AddSingleton<ISurveyRepository, CsvSurveyRepository>();

            builder.Services.AddScoped<IReviewRepository, DbReviewRepository>();

            builder.Services.AddHttpClient<ICommentRepository, ApiCommentRepository>();

            builder.Services.AddScoped<IETLService, ETLService>();
            builder.Services.AddScoped<IDataLoaderRepository, DbDataLoaderRepository>();


            var host = builder.Build();
            host.Run();
        }
    }
}