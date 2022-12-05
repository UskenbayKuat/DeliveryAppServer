using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IDriver
    {       
        public Task<List<ClientPackageInfo>> FindClientPackagesAsync(string userDriverId);
        public Task<ActionResult> SendClientPackagesToDriverAsync(string userDriverId);
        public Task<string> RejectNextFindDriverConnectionIdAsync(string driverUserId, ClientPackageInfo clientPackageInfo, CancellationToken cancellationToken);
        public Task<string> FindDriverConnectionIdAsync(ClientPackageInfo clientPackageInfo, CancellationToken cancellationToken);
    }
}