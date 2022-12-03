using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IClientPackage
    {
        public Task<ClientPackageInfo> CreateAsync(ClientPackageInfo info, string clientUserId, CancellationToken cancellationToken);
        public Task<ActionResult> GetWaitingClientPackage(string clientUserId, CancellationToken cancellationToken);
    }
}