﻿using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.Register
{
    public interface IRegistration
    {
        public Task RegisterAsync(RegistrationDto dto);

        public Task<ConfirmRegistrationDto> Confirm(ConfirmRegistrationDto dto, CancellationToken cancellationToken);
    }
}