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
    [Route("api/sewage-genomes")]
    public class SewageGenomesController : MetricsController<SewageGenomesController>
    {
        public SewageGenomesController(ILogger<SewageGenomesController> logger, ICommunicator communicator)
            : base(logger, communicator)
        { }

        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
        public Task<ActionResult<ImmutableArray<SewageGenomeDay>?>> Get(DateTime? from, DateTime? to)
        {
            return ProcessRequestAsync(communicator.GetSewageGenomesAsync, new DataFilter(from, to));
        }
    }
}
