using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Domain.Entities.Dw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.Repositories
{
    public interface IReviewRepository : IExtractor<Reviews>
    {
        
    }
}
