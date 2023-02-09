using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;

namespace ApplicationCore.Interfaces.BackgroundTaskInterfaces
{
    public interface IBackgroundTaskQueue
    {
        ValueTask QueueAsync(BackgroundOrder order);
        ValueTask<BackgroundOrder> DequeueAsync(CancellationToken cancellationToken);
    }
}