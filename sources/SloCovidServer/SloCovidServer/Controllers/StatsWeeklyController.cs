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
    [Route("api/stats-weekly")]
    public class StatsWeeklyController : MetricsController<StatsWeeklyController>
    {
        public StatsWeeklyController(ILogger<StatsWeeklyController> logger, ICommunicator communicator) : base(logger, communicator)
        {

        }
        [HttpGet]
        public async Task<ActionResult<ImmutableArray<StatsWeeklyDay>?>> Get(DateTime? from, DateTime? to)
        {
            return await ProcessRequestAsync(communicator.GetStatsWeeklyAsync, new DataFilter(from, to));
        }
    }
}
