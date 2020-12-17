using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.DB.Models;
using SloCovidServer.Handlers;
using System.Linq;
using System.Net;

namespace SloCovidServer.Controllers
{
    [Route("api/models")]
    [ApiController]
    public class ModelsController : MetricsController<ModelsController>
    {
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
        public ActionResult<int> Post()
        {
            if (User.Identity is ModelClaimsIdentity modelIdentity)
            {
                return 2;
            }
            else
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
        }
    }
}
