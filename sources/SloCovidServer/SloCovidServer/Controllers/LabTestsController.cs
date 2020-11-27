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
    [Route("api/lab-tests")]
    public class LabTestsController : MetricsController<LabTestsController>
    {
        public LabTestsController(ILogger<LabTestsController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }

        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public Task<ActionResult<ImmutableArray<LabTestDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetLabTestsAsync, new DataFilter(from, to));
        }
    }
}
