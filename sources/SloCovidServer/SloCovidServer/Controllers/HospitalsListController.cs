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
    [Route("api/hospitals-list")]
    public class HospitalsListController : MetricsController<HospitalsListController>
    {
        public HospitalsListController(ILogger<HospitalsListController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }

        [HttpGet]
        public async Task<ActionResult<ImmutableArray<Hospital>?>> Get()
        {
            return await ProcessRequestAsync(communicator.GetHospitalsListAsync, DataFilter.Empty);
        }
    }
}
