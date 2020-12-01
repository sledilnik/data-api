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
    [Route("api/daily-deaths-slovenia")]
    public class DailyDeathsSloveniaController: MetricsController<DailyDeathsSloveniaController>
    {
        public DailyDeathsSloveniaController(ILogger<DailyDeathsSloveniaController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }
        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public Task<ActionResult<ImmutableArray<DailyDeathsSlovenia>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetDailyDeathsSloveniaAsync, new DataFilter(from, to));
        }
    }
}
