using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using SloCovidServer.Services.Abstract;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        readonly ILogger<DataController> logger;
        readonly ICommunicator communicator;

        public DataController(ILogger<DataController> logger, ICommunicator communicator)
        {
            this.logger = logger;
            this.communicator = communicator;
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
            //var query = from d in data
            //            let sampleDate = new DateTime(d.Year, d.Month, d.Day)
            //            where (start.HasValue && sampleDate >= start.Value || !start.HasValue)
            //                && (end.HasValue && sampleDate <= end.Value || !end.HasValue)
            //            select d;
            //return query;
        }
    }
}
