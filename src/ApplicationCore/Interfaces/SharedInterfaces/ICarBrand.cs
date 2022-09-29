using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.SharedInterfaces
{
    public interface ICarBrand
    {
        public Task<ActionResult> SendCarBrands(CancellationToken cancellationToken);
    }
}