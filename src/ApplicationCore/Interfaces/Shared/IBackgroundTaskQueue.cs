using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;

namespace ApplicationCore.Interfaces.Shared
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueAsync(BackgroundOrder order);
        ValueTask<BackgroundOrder> DequeueAsync(CancellationToken cancellationToken);
    }
}