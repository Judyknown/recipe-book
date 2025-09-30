using Microsoft.AspNetCore.Mvc;

namespace BootCamp.Controllers
{
    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardRankController : ControllerBase
    {
        // 3.2 Get customers by rank
        // GET /leaderboard?start={start}&end={end}
        [HttpGet]
        public IActionResult GetCustomersByRank([FromQuery] int start, [FromQuery] int end)
        {
            // TODO: ???????????
            // ??????ID???????JSON??
            return Ok();
        }
    }
}
