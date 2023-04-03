﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers;

[ApiController]
[Route("api/sewage-cases")]
public class SewageWeeklyCasesController : MetricsController<SewageWeeklyCasesController>
{
    public SewageWeeklyCasesController(ILogger<SewageWeeklyCasesController> logger, ICommunicator communicator)
        : base(logger, communicator)
    { }

    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
    public Task<ActionResult<ImmutableArray<SewageWeeklyCases>?>> Get(DateTime? from, DateTime? to)
    {
        return ProcessRequestAsync(communicator.GetSewageWeeklyCasesAsync, new DataFilter(from, to));
    }
}
