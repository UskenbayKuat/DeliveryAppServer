using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IProceedRegistration
    {
        public Task<ActionResult> ProceedRegistration(ProceedRegistrationInfo info, CancellationToken cancellationToken);
    }
}