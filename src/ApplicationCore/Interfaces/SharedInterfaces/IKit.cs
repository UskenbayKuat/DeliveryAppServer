using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.SharedInterfaces
{
    public interface IKit
    {
        public Task<ActionResult> SendKits(CancellationToken cancellationToken);
    }
}