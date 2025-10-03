using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BootCamp.Controllers
{
    [ApiController]
    [Route("leaderboard")]
    public class LeaderboardRankController : ControllerBase
    {
        private readonly LeaderboardService _svc;

        public LeaderboardRankController(LeaderboardService svc)
        {
            _svc = svc; 
        }

        // GET /leaderboard?start=1&end=3
        [HttpGet]
        public IActionResult GetRange([FromQuery] int start, [FromQuery] int end)
        {
            if (start < 1 || end < start)
                return BadRequest(new ErrorDto("BadRange", "start>=1 and end>=start"));

            var list = _svc.GetRange(start, end);
            if (list == null || list.Count == 0)
                return Ok(new List<RankItem>());
            return Ok(list);
        }
    }
}
