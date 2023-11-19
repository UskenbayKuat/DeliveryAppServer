using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Register;

namespace ApplicationCore.Interfaces.Register
{
    public interface IProceedRegistration
    {
        public Task ProceedRegistration(ProceedRegistrationDto dto, Guid userId, CancellationToken cancellationToken);
    }
}