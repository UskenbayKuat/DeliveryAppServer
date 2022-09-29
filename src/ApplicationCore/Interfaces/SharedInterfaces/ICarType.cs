using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.SharedInterfaces
{
    public interface ICarType
    {
        public Task<ActionResult> SendCarTypes(CancellationToken cancellationToken);
    }
}