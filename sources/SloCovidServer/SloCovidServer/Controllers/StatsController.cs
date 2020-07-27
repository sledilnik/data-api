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
    [Route("api/[controller]")]
    public class StatsController : MetricsController<StatsController>
    {
        public StatsController(ILogger<StatsController> logger, ICommunicator communicator) 
            : base(logger, communicator)
        {} 

        [HttpGet]
        public async Task<ActionResult<ImmutableArray<StatsDaily>?>> Get(DateTime? from, DateTime? to)
        {
            return await ProcessRequestAsync(communicator.GetStatsAsync, new DataFilter(from, to));
        }
    }
}
