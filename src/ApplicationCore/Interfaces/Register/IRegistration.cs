using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.Register
{
    public interface IRegistration
    {
        public Task<ActionResult> SendTokenAsync(RegistrationDto dto);

        public Task<ActionResult> Confirm(ConfirmRegistrationDto dto, CancellationToken cancellationToken);
    }
}