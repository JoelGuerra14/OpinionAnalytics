using OpinionAnalytics.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.Repositories
{
    public interface IDwhRepository
    {
        Task LoadDimDataAsync(DimEntities entidades);
    }
}
