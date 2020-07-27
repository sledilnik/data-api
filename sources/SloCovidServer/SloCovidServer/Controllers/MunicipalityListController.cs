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
        public async Task<ActionResult<ImmutableArray<Municipality>?>> Get()
        {
            return await ProcessRequestAsync(communicator.GetMunicipalitiesListAsync, DataFilter.Empty);
        }
    }
}
