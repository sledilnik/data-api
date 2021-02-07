using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
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
        public ActionResult<ImmutableDictionary<string, SchoolStatus>> Get([FromQuery(Name = "id")]string[] schoolIds, DateTime? from, DateTime? to)
        {
            var result = communicator.GetSchoolsStatuses(RequestETag, new SchoolsStatusesFilter(schoolIds.ToImmutableArray(), from, to));
            if (result.Summary is null && result.ETag is not null)
            {
                return StatusCode((int)HttpStatusCode.NotModified);
            }
            Response.Headers[HeaderNames.ETag] = result.ETag;
            return result.Summary;
        }
    }
}
