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
    [Route("api/health-centers")]
    public class HealthCentersController : MetricsController<HealthCentersController>
    {
        public HealthCentersController(ILogger<HealthCentersController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] {"*"}, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<HealthCentersDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetHealthCentersAsync, new DataFilter(from, to));
        }
    }
}
