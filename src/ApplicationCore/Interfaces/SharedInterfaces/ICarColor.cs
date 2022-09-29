using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.SharedInterfaces
{
    public interface ICarColor
    {
        public Task<ActionResult> SendCarColors(CancellationToken cancellationToken);
    }
}