using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces
{
    public interface IRegistration
    {
        public Task<ActionResult> SendTokenAsync(RegistrationToken token);

        public Task<ActionResult> Confirm(ConfirmRegistrationToken token, CancellationToken cancellationToken);
    }
}