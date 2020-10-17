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
    [Route("api/municipalities")]
    public class MunicipalitiesController : MetricsController<MunicipalitiesController>
    {
        public MunicipalitiesController(ILogger<MunicipalitiesController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] {"*"}, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<MunicipalityDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetMunicipalitiesAsync, new DataFilter(from, to));
        }
    }
}
