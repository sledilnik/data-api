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
    [Route("api/age-daily-deaths-slovenia")]
    public class AgeDailyDeathsSloveniaController : MetricsController<AgeDailyDeathsSloveniaController>
    {
        public AgeDailyDeathsSloveniaController(ILogger<AgeDailyDeathsSloveniaController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }
        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public Task<ActionResult<ImmutableArray<AgeDailyDeathsSloveniaDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetAgeDailyDeathsSloveniaAsync, new DataFilter(from, to));
        }
    }
}
