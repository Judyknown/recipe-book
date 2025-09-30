using BootCamp.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var newScore = LeaderboardData.Leaderboard.AddOrUpdate(
                customerid,
                score,                        // initial value if customer does not exist
                (id, oldScore) => oldScore + score // update function if customer exists
            );

            // Use LINQ to keep a sorted view (by score desc, then customerId asc)
            var sortedLeaderboard = LeaderboardData.Leaderboard
                .Where(x => x.Value > 0)                // only scores > 0 participate
                .OrderByDescending(x => x.Value)        // higher score first
                .ThenBy(x => x.Key)                     // if tie, smaller customerId first
                .ToList();

            // (Optional) find this customer's rank
            var rank = sortedLeaderboard
                .Select((entry, index) => new { entry.Key, Rank = index + 1 })
                .FirstOrDefault(x => x.Key == customerid)?.Rank;

            // Requirement: response = current score
            // But you can also debug rank inside if needed
            return Ok(newScore);
        }
    }
}
