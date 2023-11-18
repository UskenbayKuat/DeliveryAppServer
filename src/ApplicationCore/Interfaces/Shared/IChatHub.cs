using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces.Shared
{
    public interface IChatHub
    {
        public Task ConnectedAsync(Guid? userId, string connectId);
        public Task DisconnectedAsync(Guid? userId, string connectId);
        public Task<string> GetConnectionIdAsync(Guid? userId, CancellationToken cancellationToken);
        public Task<List<string>> GetConnectionIdListAsync(Guid driverUserId);
    }
}