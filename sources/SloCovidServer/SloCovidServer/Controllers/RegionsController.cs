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
    [Route("api")]
    public class RegionsController : ControllerBase
    {
        readonly ILogger<RegionsController> logger;
        readonly ICommunicator communicator;

        public RegionsController(ILogger<RegionsController> logger, ICommunicator communicator)
        {
            this.logger = logger;
            this.communicator = communicator;
        }

        [HttpGet]
        [Route("regions")]
        public async Task<ActionResult<ImmutableArray<RegionsDay>?>> Get()
        {
            string etag = null;

            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etagValues))
            {
                etag = etagValues.SingleOrDefault() ?? "";
            }
            var result = await communicator.GetRegionsAsync(etag, CancellationToken.None);
            Response.Headers[HeaderNames.ETag] = result.ETag;
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

        [HttpGet]
        [Route("regions-pivot")]
        public async Task<ActionResult<ImmutableArray<ImmutableArray<object>>?>> GetPivot()
        {
            string etag = null;

            if (Request.Headers.TryGetValue(HeaderNames.IfNoneMatch, out var etagValues))
            {
                etag = etagValues.SingleOrDefault() ?? "";
            }
            var result = await communicator.GetRegionsPivotAsync(etag, CancellationToken.None);
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
