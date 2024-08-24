using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;

namespace SloCovidServer.Controllers;

[ApiController]
[Route("api/opsi-cases")]
public class OpsiCasesController : MetricsController<OpsiCasesController>
{
    public OpsiCasesController(ILogger<OpsiCasesController> logger, ICommunicator communicator) : base(logger, communicator)
    {
    }
    [HttpGet]
    [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
    public Task<ActionResult<ImmutableArray<OpsiCase>?>> Get(DateTime? from, DateTime? to)
    {
        return ProcessRequestAsync(communicator.GetOpsiCasesAsync, new DataFilter(from, to));
    }
}