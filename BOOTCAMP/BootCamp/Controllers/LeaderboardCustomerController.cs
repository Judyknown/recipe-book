using Microsoft.AspNetCore.Mvc;

namespace BootCamp.Controllers
{
    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardCustomerController : ControllerBase
    {
        // 3.3 Get customers by customerid
        // GET /leaderboard/{customerid}?high={high}&low={low}
        [HttpGet("{customerid:long}")]
        public IActionResult GetCustomerWithNeighbors(long customerid, [FromQuery] int high = 0, [FromQuery] int low = 0)
        {
            // TODO: ??????????????
            // ??????????JSON??
            return Ok();
        }
    }
}
