using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Persistence.Db.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Persistence.Api
{
    public class ApiCommentRepository : ICommentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiCommentRepository> _logger;
        private readonly string _baseUrl;

        public ApiCommentRepository(HttpClient httpClient, IConfiguration config, ILogger<ApiCommentRepository> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = config["DataSources:Comments:ApiUrl"]
                ?? throw new InvalidOperationException("No se encontró la URL base para la API de comentarios.");
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Llamando API de comentarios: {Url}", _baseUrl);

                var response = await _httpClient.GetAsync($"{_baseUrl}/api/CommentApi/GetAllComments");
                response.EnsureSuccessStatusCode();

                var comments = await response.Content.ReadFromJsonAsync<IEnumerable<Comment>>()
                               ?? Enumerable.Empty<Comment>();

                _logger.LogInformation("Se extrajeron {Count} comentarios desde la API.", comments.Count());
                return comments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consumir la API de comentarios.");
                return Enumerable.Empty<Comment>();
            }
        }
    }
}
