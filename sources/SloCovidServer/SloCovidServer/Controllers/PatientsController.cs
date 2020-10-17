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
    [Route("api/[controller]")]
    public class PatientsController : MetricsController<PatientsController>
    {
        public PatientsController(ILogger<PatientsController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }

        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] {"*"}, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<PatientsDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetPatientsAsync, new DataFilter(from, to));
        }
    }
}
