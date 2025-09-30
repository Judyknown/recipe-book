using BootCamp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            if (start <= 0 || end < start)
            {
                return BadRequest("Invalid rank range.");
            }

            var rankedLeaderboard = LeaderboardData.Leaderboard
                .OrderByDescending(pair => pair.Value)
                .Select((pair, index) => new
                {
                    customerId = pair.Key,
                    score = pair.Value,
                    rank = index + 1
                })
                .ToList();

            var result = rankedLeaderboard
                .Where(c => c.rank >= start && c.rank <= end)
                .ToList();

            return Ok(result);
        }
    }
}
