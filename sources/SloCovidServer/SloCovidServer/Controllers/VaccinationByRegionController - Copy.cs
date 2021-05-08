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
    [Route("api/vaccination-by-municipality")]
    public class VaccinationByMunicipalityController : MetricsController<VaccinationByMunicipalityController>
    {
        public VaccinationByMunicipalityController(ILogger<VaccinationByMunicipalityController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }
        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public Task<ActionResult<ImmutableArray<VaccinationByMunicipalityDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetVaccinationByMunicipalityAsync, new DataFilter(from, to));
        }
    }
}
