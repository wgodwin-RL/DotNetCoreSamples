using System.Threading;
using System.Threading.Tasks;

namespace LD.Models.Interfaces
{
    public interface IConsumerAppService
    {
        Task Run(CancellationToken cancellationToken);
    }
}
