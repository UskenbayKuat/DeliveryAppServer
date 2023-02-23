using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;

namespace Infrastructure.Services.BackgroundServices
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<BackgroundOrder> _queue;

        public BackgroundTaskQueue()
        {
            _queue = Channel.CreateUnbounded<BackgroundOrder>();
        }

        public async ValueTask QueueAsync(BackgroundOrder order)
            => await _queue.Writer.WriteAsync(order);


        public async ValueTask<BackgroundOrder> DequeueAsync(CancellationToken cancellationToken)
            => await _queue.Reader.ReadAsync(cancellationToken);
    }
}