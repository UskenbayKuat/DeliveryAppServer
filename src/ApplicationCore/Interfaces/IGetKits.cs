using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IGetKits
    {
        public Task<ActionResult> SendKits(CancellationToken cancellationToken);
    }
}