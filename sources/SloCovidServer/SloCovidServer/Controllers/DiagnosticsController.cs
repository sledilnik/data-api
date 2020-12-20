using Microsoft.AspNetCore.Mvc;

namespace SloCovidServer.Controllers
{
    [ApiController]
    public class DiagnosticsController : Controller
    {
        [HttpHead]
        [HttpGet]
        [Route("diagnostics/readyz")]
        public IActionResult Ready()
        {
            return Ok();
        }
    }
}
