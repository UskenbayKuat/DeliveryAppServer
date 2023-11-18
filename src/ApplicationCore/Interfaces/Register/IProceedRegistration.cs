using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Register;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.Register
{
    public interface IProceedRegistration
    {
        public Task<ActionResult> ProceedRegistration(ProceedRegistrationDto dto, Guid userId, CancellationToken cancellationToken);
    }
}