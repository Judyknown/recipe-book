using BootCamp.Data;
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
            // Validate parameters
            if (customerid <= 0)
            {
                return BadRequest("customerid must be a positive int64.");
            }

            if (score < -1000 || score > 1000)
            {
                return BadRequest("score must be in range [-1000, 1000].");
            }

            // Insert if not exists, otherwise update by adding the new score
            LeaderboardData.Leaderboard.AddOrUpdate(
                customerid,
                score,                        // initial value if customer does not exist
                (id, oldScore) => oldScore + score // update function if customer exists
            );

            // Return the updated score
            return Ok(LeaderboardData.Leaderboard[customerid]);
        }
    }
}
