using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces.RegisterInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.RegisterServices
{
    public class RegisterBySmsService : IRegistration
    {
        public Task<ActionResult> SendTokenAsync(RegistrationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<ConfirmRegistrationInfo> Confirm(ConfirmRegistrationInfo info, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}