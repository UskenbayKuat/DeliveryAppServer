using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Register;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.RegisterInterfaces
{
    public interface IProceedRegistration
    {
        public Task<ActionResult> ProceedRegistration(ProceedRegistrationDto dto, string userId, CancellationToken cancellationToken);
    }
}