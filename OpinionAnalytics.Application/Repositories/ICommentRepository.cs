using OpinionAnalytics.Application.Interfaces;
using OpinionAnalytics.Domain.Entities.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpinionAnalytics.Application.Interfaces
{
    public interface ICommentRepository : IExtractor<Comment>
    {

    }
}
