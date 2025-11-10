using OpinionAnalytics.Domain.Entities.Api;
using OpinionAnalytics.Domain.Entities.Csv;
using OpinionAnalytics.Domain.Entities.Dw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.Repositories
{
    public interface IDataLoaderRepository
    {
        Task SaveSurveysAsync(IEnumerable<Survey> surveys);
        Task SaveReviewsAsync(IEnumerable<Reviews> reviews);
        Task SaveCommentsAsync(IEnumerable<Comment> comments);
    }
}
