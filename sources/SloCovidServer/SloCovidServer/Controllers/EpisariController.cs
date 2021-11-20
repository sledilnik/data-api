using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/episari-nijz-weekly")]
    public class EpisariController : MetricsController<EpisariController>
    {
        public EpisariController(ILogger<EpisariController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }

        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<EpisariWeek>?>> Get()
        {
            return ProcessRequestAsync(communicator.GetEpisariWeeksAsync, DataFilter.Empty);
        }
    }
}
