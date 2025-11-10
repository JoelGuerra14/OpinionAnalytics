using Microsoft.EntityFrameworkCore;
using OpinionAnalytics.Api.Data.Context;
using OpinionAnalytics.Api.Data.Interface;
using OpinionAnalytics.Application.Repositories;
using OpinionAnalytics.Domain.Entities.Api;

namespace OpinionAnalytics.Api.Data.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly CommentContext _context;

        public CommentRepository(CommentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
