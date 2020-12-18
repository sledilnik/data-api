using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
using System.Net;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/summary")]
    public class SummaryController : MetricsController<SummaryController>
    {
        public SummaryController(ILogger<SummaryController> logger, ICommunicator communicator) : base(logger, communicator)
        {}

        [HttpGet]
        [ResponseCache(CacheProfileName = nameof(CacheProfiles.Default60))]
        public ActionResult<Summary> Get(DateTime? toDate)
        {
            // supports only json for now
            //if (!IsJsonRequested && !IsAnyRequested)
            //{
            //    return StatusCode((int)HttpStatusCode.NotAcceptable);
            //}
            var result = communicator.GetSummary(RequestETag, toDate);
            if (result.Summary is null && result.ETag is not null)
            {
                return StatusCode((int)HttpStatusCode.NotModified);
            }
            Response.Headers[HeaderNames.ETag] = result.ETag;
            return result.Summary;
        }
    }
}
