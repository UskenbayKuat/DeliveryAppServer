﻿using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.RegisterInterfaces
{
    public interface IProceedRegistration
    {
        public Task<ActionResult> ProceedRegistration(ProceedRegistrationInfo info, string userId, CancellationToken cancellationToken);
    }
}