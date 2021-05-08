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
    [Route("api/vaccination-by-age")]
    public class VaccinationByAgeController : MetricsController<VaccinationByAgeController>
    {
        public VaccinationByAgeController(ILogger<VaccinationByAgeController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }
        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public Task<ActionResult<ImmutableArray<VaccinationByAgeDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetVaccinationByAgeAsync, new DataFilter(from, to));
        }
    }
}
