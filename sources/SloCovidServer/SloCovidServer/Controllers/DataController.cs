using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SloCovidServer.Services.Abstract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    /// <summary>
    /// Legacy controller, will be removed eventually.
    /// Use <see cref="StatsController"/> instead.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        readonly ILogger<DataController> logger;
        readonly ICommunicator communicator;
        readonly ISlackService slackService;

        public DataController(ILogger<DataController> logger, ICommunicator communicator, ISlackService slackService)
        {
            this.logger = logger;
            this.communicator = communicator;
            this.slackService = slackService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            string etag = null;

            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etagValues))
            {
                etag = etagValues.SingleOrDefault() ?? "";
            }
            var result = await communicator.GetStatsAsync(etag, CancellationToken.None);
            Response.Headers[HeaderNames.ETag] = result.ETag;
            Response.Headers["SchemaVersion"] = "1";
            if (result.Data.HasValue)
            {
                return Ok(result.Data);
            }
            else
            {
                return StatusCode(304);
            }
        }

        //[HttpGet]
        //[Route("slack")]
        //public async Task Test()
        //{
        //    throw new System.Exception("Test");
        //    await slackService.SendNotificationAsync("Testing 1..2..3.. from production", default);
        //}
    }
}
