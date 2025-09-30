using Microsoft.AspNetCore.Mvc;

namespace BootCamp.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        // 3.1 Update Score
        // POST /customer/{customerid}/score/{score}
        [HttpPost("{customerid:long}/score/{score:decimal}")]
        public IActionResult UpdateScore(long customerid, decimal score)
        {
            // TODO: ????????
            // ??????????
            return Ok();
        }
    }
}
