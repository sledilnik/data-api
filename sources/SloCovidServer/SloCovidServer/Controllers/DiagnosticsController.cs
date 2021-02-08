using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SloCovidServer.Controllers
{
    [ApiController]
    public class DiagnosticsController : Controller
    {
        [HttpHead("diagnostics/readyz", Name = "ReadyHead")]
        [HttpGet("diagnostics/readyz", Name = "ReadyGet")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(void))]
        public IActionResult Ready()
        {
            return Ok();
        }
    }
}
