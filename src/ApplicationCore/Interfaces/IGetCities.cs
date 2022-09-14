using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IGetCities
    {
        public Task<ActionResult> SendCities(CancellationToken cancellationToken);
    }
}