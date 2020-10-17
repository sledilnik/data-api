using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/retirement-homes")]
    public class RetirementHomesController : MetricsController<RetirementHomesController>
    {
        public RetirementHomesController(ILogger<RetirementHomesController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] {"*"}, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<RetirementHomesDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetRetirementHomesAsync, new DataFilter(from, to));
        }
    }
}
