using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System.Collections.Immutable;
using System.Net;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/school-status")]
    public class SchoolStatusController : MetricsController<SchoolStatusController>
    {
        public SchoolStatusController(ILogger<SchoolStatusController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }

        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public ActionResult<ImmutableDictionary<int, SchoolStatus>> Get([FromQuery(Name = "id")]int[] schoolIds)
        {
            var result = communicator.GetSchoolsStatuses(RequestETag, schoolIds.ToImmutableArray());
            if (result.Summary is null && result.ETag is not null)
            {
                return StatusCode((int)HttpStatusCode.NotModified);
            }
            Response.Headers[HeaderNames.ETag] = result.ETag;
            return result.Summary;
        }
    }
}
