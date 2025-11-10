
using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Api.Data.Context;
using OpinionAnalytics.Api.Data.Interface;
using OpinionAnalytics.Api.Data.Repository;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Persistence.Api;

namespace OpinionAnalytics.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<CommentContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("OltpConnection")));

            builder.Services.AddScoped<ICommentRepository, CommentRepository>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();
            app.Run();
        }
    }
}
