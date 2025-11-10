using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpinionAnalytics.Application.Interfaces
{
    public interface IETLService
    {
        Task RunAsync(CancellationToken cancellationToken);
    }
}
