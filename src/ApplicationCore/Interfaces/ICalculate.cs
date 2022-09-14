using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Entities.AppEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface ICalculate
    {
        public Task<ActionResult> Calculate(ClientPackageInfo info,CancellationToken cancellationToken);
    }
}