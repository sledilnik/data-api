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
    [Route("api/schools")]
    public class SchoolCasesController : MetricsController<SchoolCasesController>
    {
        public SchoolCasesController(ILogger<SchoolCasesController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }

        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<SchoolCasesDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetSchoolCasesAsync, new DataFilter(from, to));
        }
    }
}
