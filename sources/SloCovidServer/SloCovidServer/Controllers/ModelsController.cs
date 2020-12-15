using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SloCovidServer.DB.Models;
using System.Linq;

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
    }
}
