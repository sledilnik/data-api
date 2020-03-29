using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HospitalsController : ControllerBase
    {
        readonly ILogger<HospitalsController> logger;
        readonly ICommunicator communicator;

        public HospitalsController(ILogger<HospitalsController> logger, ICommunicator communicator)
        {
            this.logger = logger;
            this.communicator = communicator;
        }

        [HttpGet]
        public async Task<ActionResult<ImmutableArray<HospitalsDay>?>> Get()
        {
            string etag = null;

            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etagValues))
            {
                etag = etagValues.SingleOrDefault() ?? "";
            }
            var result = await communicator.GetHospitalsAsync(etag, CancellationToken.None);
            Response.Headers[HeaderNames.ETag] = result.ETag;
            if (result.Data.HasValue)
            {
                return Ok(result.Data);
            }
            else
            {
                return StatusCode(304);
            }
        }
    }
}
