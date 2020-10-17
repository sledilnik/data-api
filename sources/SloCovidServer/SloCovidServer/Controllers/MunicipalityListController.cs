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
    [Route("api/municipalities-list")]
    public class MunicipalityListController : MetricsController<MunicipalityListController>
    {
        public MunicipalityListController(ILogger<MunicipalityListController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }

        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] {"*"}, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<Municipality>?>> Get()
        {
            return ProcessRequestAsync(communicator.GetMunicipalitiesListAsync, DataFilter.Empty);
        }
    }
}
