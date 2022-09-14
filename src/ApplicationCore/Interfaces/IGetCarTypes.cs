using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IGetCarTypes
    {
        public Task<ActionResult> SendCarTypes(CancellationToken cancellationToken);
    }
}