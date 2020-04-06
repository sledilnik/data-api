using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/retirement-homes-list")]
    public class RetirementHomesListController: MetricsController<RetirementHomesListController>
    {
        public RetirementHomesListController(ILogger<RetirementHomesListController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        public async Task<ActionResult<ImmutableArray<RetirementHome>?>> Get()
        {
            return await ProcessRequestAsync(communicator.GetRetirementHomesListAsync);
        }
    }
}
