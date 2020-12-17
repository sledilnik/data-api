using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SloCovidServer.DB.Models;
using SloCovidServer.Handlers;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SloCovidServer.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class ModelsController : MetricsController<ModelsController>
    {
        public record PostData
        {
            public DateTimeOffset Date { get; init; }
            public string Scenario { get; init; }
            public string IntervalType { get; init; }
            public int? IntervalWidth { get; init; }
        }
        readonly DataContext dataContext;
        public ModelsController(ILogger<ModelsController> logger, DataContext dataContext): base(logger, communicator: default)
        {
            this.dataContext = dataContext;
        }
        [HttpGet]
        public ActionResult<int> Get()
        {
            return dataContext.ModelsModels.Count();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromForm] PostData data)
        {
            if (User.Identity is ModelClaimsIdentity modelIdentity)
            {
                try
                {
                    var prediction = await GetModelPredictionAsync(data);
                    prediction.ModelId = modelIdentity.ModelId;
                    using (var transaction = await dataContext.Database.BeginTransactionAsync())
                    {
                        dataContext.ModelsPredictions.Add(prediction);
                        await dataContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
        }

        async Task<ModelsPrediction> GetModelPredictionAsync(PostData data)
        {
            ModelsPredictionintervaltype intervalType = null;
            if (!string.IsNullOrEmpty(data.IntervalType))
            {
                intervalType = await dataContext.ModelsPredictionintervaltypes.Where(it => it.Name == data.IntervalType).SingleOrDefaultAsync()
                    ?? throw new Exception($"Invalid interval type {data.IntervalType}");
            }
            ModelsPredictionintervalwidth intervalWidth = null;
            if (data.IntervalWidth is not null)
            {
                intervalWidth = await dataContext.ModelsPredictionintervalwidths.Where(iw => iw.Width == data.IntervalWidth).SingleOrDefaultAsync()
                    ?? throw new Exception($"Invalid interval width {data.IntervalWidth}");
            }
            return new ModelsPrediction
            {
                Date = data.Date.Date,
                Scenario = await dataContext.ModelsScenarios.Where(m => m.Name == data.Scenario).SingleOrDefaultAsync()
                    ?? throw new Exception($"Invalid scenario {data.Scenario}"),
                IntervalType = intervalType,
                IntervalWidth = intervalWidth
            };
        }
    }
}
