using OpinionAnalytics.Application.DTOs;
using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
using OpinionAnalytics.Domain.Entities.Dwh.Dimensions;
using OpinionAnalytics.Domain.Entities.Dwh.Facts;

namespace OpinionAnalytics.Application.Repositories
{
    public interface IDwhRepository
    {

        Task LoadStagingAsync(IEnumerable<Survey> surveys, IEnumerable<Reviews> reviews, IEnumerable<Comment> comments);
        IQueryable<Survey> GetSurveysQuery();
        IQueryable<Reviews> GetReviewsQuery();
        IQueryable<Comment> GetCommentsQuery();

        IQueryable<DimCliente> GetDimClientesQuery();
        IQueryable<DimProducto> GetDimProductosQuery();
        IQueryable<DimDate> GetDimDatesQuery();
        Task<List<DimFuente>> GetDimFuentesListAsync();
        Task<List<DimClasificacion>> GetDimClasificacionesListAsync();
        Task LoadDimDataAsync(DimEntities entidades);
        Task SaveFactsAsync(List<FactOpinion> facts);
    }
}