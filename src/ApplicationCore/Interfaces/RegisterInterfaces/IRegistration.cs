using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.RegisterInterfaces
{
    public interface IRegistration
    {
        public Task<ActionResult> SendTokenAsync(RegistrationToken token);

        public Task<ConfirmRegistrationInfo> Confirm(ConfirmRegistrationInfo info, CancellationToken cancellationToken);
    }
}