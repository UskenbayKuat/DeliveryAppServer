using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services
{
    public class RegisterBySms : IRegistration
    {
        public Task<ActionResult> SendTokenAsync(RegistrationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> Confirm(ConfirmRegistrationToken token, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}