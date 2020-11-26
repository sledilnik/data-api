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
    [Route("api/monthly-deaths-slovenia")]
    public class MonthlyDeathsSloveniaController: MetricsController<MonthlyDeathsSloveniaController>
    {
        public MonthlyDeathsSloveniaController(ILogger<MonthlyDeathsSloveniaController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }
        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public Task<ActionResult<ImmutableArray<MonthlyDeathsSlovenia>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetMonthlyDeathsSloveniaAsync, new DataFilter(from, to));
        }
    }
}
