using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BootCamp.Controllers
{
    public class LeaderboardCustomer
    {
        public long CustomerId { get; set; }
        public decimal Score { get; set; }
        public int Rank { get; set; }
    }

    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardCustomerController : ControllerBase
    {
        private readonly LeaderboardService _svc;
        public LeaderboardCustomerController(LeaderboardService svc) => _svc = svc;

        // GET /leaderboard/{customerid}?high={high}&low={low}
        [HttpGet("{customerid:long}")]
        public IActionResult GetCustomerWithNeighbors(long customerid, [FromQuery] int high = 0, [FromQuery] int low = 0)
        {
            if (high < 0) high = 0;
            if (low < 0) low = 0;

            var result = _svc.GetWithNeighbors(customerid, high, low);
            if (result is null)
                return NotFound($"CustomerId {customerid} not found or not ranked.");

            // Compose list in the same format as original implementation: high neighbors + self + low neighbors
            var list = new List<LeaderboardCustomer>(high + 1 + low);
            list.AddRange(result.High.Select(r => new LeaderboardCustomer { CustomerId = r.CustomerId, Score = r.Score, Rank = r.Rank }));
            list.Add(new LeaderboardCustomer { CustomerId = result.Customer.CustomerId, Score = result.Customer.Score, Rank = result.Customer.Rank });
            list.AddRange(result.Low.Select(r => new LeaderboardCustomer { CustomerId = r.CustomerId, Score = r.Score, Rank = r.Rank }));

            return Ok(list);
        }
    }
}