using Microsoft.AspNetCore.Mvc;

namespace Kurier.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Check()
        {
            return Ok("It's ALIVE!!!");
        }
    }
}
