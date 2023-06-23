﻿using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Models.Dtos.Register;
using ApplicationCore.Models.Values;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Services.RegisterServices
{
    public class RegisterBySmsService : IRegistration
    {
        public Task<ActionResult> SendTokenAsync(RegistrationDto dto)
        {
            throw new System.NotImplementedException();
        }

        public Task<ActionResult> Confirm(ConfirmRegistrationDto dto, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}