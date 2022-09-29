using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface IClientPackage
    {
        public Task<ActionResult> CreateClientPackage(ClientPackageInfo info, CancellationToken cancellationToken);
    }
}