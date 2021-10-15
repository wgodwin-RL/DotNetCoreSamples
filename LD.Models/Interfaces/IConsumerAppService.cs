using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LD_Models.Interfaces
{
    public interface IConsumerAppService
    {
        Task Run(CancellationToken cancellationToken);
    }
}
