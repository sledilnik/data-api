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
    [Route("api/deceased-regions")]
    public class DeceasedPerRegionsController : MetricsController<DeceasedPerRegionsController>
    {
        public DeceasedPerRegionsController(ILogger<DeceasedPerRegionsController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ImmutableArray<DeceasedPerRegionsDay>?>> Get(DateTime? from, DateTime? to)
        {
            return await ProcessRequestAsync(communicator.GetDeceasedPerRegionsAsync, new DataFilter(from, to));
        }
    }
}
