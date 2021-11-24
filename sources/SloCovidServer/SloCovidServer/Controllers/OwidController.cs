using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using SloCovidServer.Models;
using SloCovidServer.Models.Owid;
using SloCovidServer.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

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
#if !DEBUG
        [ResponseCache(VaryByQueryKeys = new[] { "*" }, VaryByHeader = "Accept", Duration = 60)]
#endif
        public ActionResult<OwidCountryDay[]> Get(DateTime? from, DateTime? to, string countries, string columns)
        {
            ImmutableDictionary<string, object> CreateOutput(string isoCode, Country c, CountryData d, ImmutableHashSet<string> columns)
            {
                var result = new Dictionary<string, object> {
                            { "isoCode", isoCode },
                            { "date", d.Date }
                        };
                foreach (string column in columns)
                {
                    if (c.AllColumns.TryGetValue(column, out var countryValue))
                    {
                        result.Add(column, countryValue);
                    }
                    else if (d.AllColumns.TryGetValue(column, out var dataValue))
                    {
                        result.Add(column, dataValue);
                    }
                    else
                    {
                        result.Add(column, null);
                    }
                }
                return result.ToImmutableDictionary();
            }
            string etag = RequestETag;
            var cache = communicator.GetOwidCountries(etag);
            if (!cache.HasValue)
            {
                return StatusCode(500);
            }
            var data = cache.Value.Data;
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
            var validColumns = !string.IsNullOrEmpty(columns) ?
                // TODO possible improvement - switch to Span<T> for split
                ImmutableHashSet.Create(StringComparer.OrdinalIgnoreCase, columns.Split(',').Where(c => c != "date" && c != "isoCode").ToArray())
                : null;
            var query = from c in data
                        where validCountries == null || validCountries.Contains(c.Key)
                        from d in c.Value.Data
                        where (!@from.HasValue || d.Date >= @from.Value) && (!to.HasValue || d.Date <= to)
                        select CreateOutput(c.Key, c.Value, d, validColumns);
                        //select new OwidCountryDay(
                        //    d.Date,
                        //    isoCode: c.Key,
                        //    d.NewCases, d.NewCasesPerMillion,
                        //    d.TotalCases, d.TotalCasesPerMillion,
                        //    d.TotalDeaths, d.TotalDeathsPerMillion
                        //);
            return Ok(query.ToArray());
        }
    }
}
