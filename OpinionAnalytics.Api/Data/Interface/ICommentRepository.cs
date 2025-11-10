using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Domain.Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpinionAnalytics.Api.Data.Interface
{
    public interface ICommentRepository : IExtractor<Comment>
    {

    }
}
