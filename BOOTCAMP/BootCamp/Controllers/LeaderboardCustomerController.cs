using Microsoft.AspNetCore.Mvc;

namespace BootCamp.Controllers
{
    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardCustomerController : ControllerBase
    {
        private readonly LeaderboardService _svc;
        public LeaderboardCustomerController(LeaderboardService svc) => _svc = svc;

        // 3.3 Get customers by customerid with neighbors
        // GET /leaderboard/{customerid}?high={high}&low={low}
        [HttpGet("{customerid:long}")]
        public IActionResult GetCustomerWithNeighbors(long customerid, [FromQuery] int high = 0, [FromQuery] int low = 0)
        {
            if (customerid <= 0)
                return BadRequest(new ErrorDto("BadCustomerId", "customerid must be positive"));

            var result = _svc.GetWithNeighbors(customerid, high, low);
            if (result is null)
                return NotFound(new ErrorDto("NotFound", "customer not in leaderboard (score <=0 or never updated)"));

            return Ok(new { customer = result.Customer, high = result.High, low = result.Low });
        }
    }
}
