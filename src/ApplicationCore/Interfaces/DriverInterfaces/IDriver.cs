using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.Values;

namespace ApplicationCore.Interfaces.DriverInterfaces
{
    public interface IDriver
    {       
        public Task<List<ClientPackageInfoToDriver>> FindClientPackagesAsync(string userId);
        public Task<string> RejectNextFindDriverConnectionIdAsync(string driverUserId, ClientPackageInfoToDriver clientPackageInfoToDriver, CancellationToken cancellationToken);
        public Task<string> FindDriverConnectionIdAsync(ClientPackageInfoToDriver clientPackageInfoToDriver, CancellationToken cancellationToken);
        
    }
}