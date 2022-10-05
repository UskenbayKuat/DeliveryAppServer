﻿using System.Threading;
using System.Threading.Tasks;
using ApplicationCore.Entities.ApiEntities;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.ClientInterfaces
{
    public interface ICalculate
    {
        public Task<ActionResult> Calculate(ClientPackageInfo info,CancellationToken cancellationToken);
    }
}