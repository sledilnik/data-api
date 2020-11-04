using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Namotion.Reflection;
using SloCovidServer.Models;
using SloCovidServer.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OwidController : MetricsController<OwidController>
    {
        public OwidController(ILogger<OwidController> logger, ICommunicator communicator) : base(logger, communicator)
        {
        }
        [HttpGet]
        [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
        public ActionResult<ImmutableArray<OwidCountryDay>?> Get(DateTime? from, DateTime? to, string countries) //[FromQuery(Name = "countries")]string[] countries
        {
            string etag = RequestETag;
            var cache = communicator.GetOwidCountries(etag);
            var data = cache.Data;
            if (data == null)
            {
                if (!string.IsNullOrEmpty(etag))
                {
                    return StatusCode(304);
                }
                else
                {
                    return NotFound();
                }
            }
            var validCountries = !string.IsNullOrEmpty(countries) ? 
                // TODO possible improvement - switch to Span<T> for split
                ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, countries.Split(','))
                : null;
            var query = from c in data
                        where validCountries == null || validCountries.Contains(c.Key)
                        from d in c.Value.Data
                        where (!@from.HasValue || d.Date >= @from.Value) && (!to.HasValue || d.Date <= to)
                        select new OwidCountryDay(
                            d.Date, 
                            isoCode: c.Key, 
                            d.NewCases, d.NewCasesPerMillion, 
                            d.TotalCases, d.TotalCasesPerMillion,
                            d.TotalDeaths, d.TotalDeathsPerMillion
                        );
            return Ok(query.ToImmutableArray());
        }
    }
}
