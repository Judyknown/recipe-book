using Microsoft.AspNetCore.Mvc;

namespace BootCamp.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private readonly LeaderboardService _svc;
        public CustomerController(LeaderboardService svc) => _svc = svc;

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

            var newScore = _svc.UpdateScore(customerid, score);

            return Ok(newScore);
        }
    }
}
