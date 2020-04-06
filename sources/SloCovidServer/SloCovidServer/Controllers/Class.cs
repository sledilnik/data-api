using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/retirement-homes")]
    public class RetirementHomesController : MetricsController<RetirementHomesController>
    {
        public RetirementHomesController(ILogger<RetirementHomesController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ImmutableArray<RetirementHomesDay>?>> Get()
        {
            return await ProcessRequestAsync(communicator.GetRetirementHomesAsync);
        }
    }
}
