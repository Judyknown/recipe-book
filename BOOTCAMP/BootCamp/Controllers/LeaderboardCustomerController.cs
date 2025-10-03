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
        // 全局分数表，前两个功能会维护这个表
        private static readonly Dictionary<long, decimal> ScoreTable = new();

        // 供分数更新和按排名查询功能调用
        public static void UpdateScore(long customerId, decimal delta)
        {
            if (ScoreTable.ContainsKey(customerId))
                ScoreTable[customerId] += delta;
            else
                ScoreTable[customerId] = delta;
        }

        public static List<LeaderboardCustomer> GetLeaderboard()
        {
            // 只统计分数大于0的客户
            var ranked = ScoreTable
                .Where(kv => kv.Value > 0)
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => kv.Key)
                .Select((kv, idx) => new LeaderboardCustomer
                {
                    CustomerId = kv.Key,
                    Score = kv.Value,
                    Rank = idx + 1
                })
                .ToList();
            return ranked;
        }

        // 你负责的接口
        // GET /leaderboard/{customerid}?high={high}&low={low}
        [HttpGet("{customerid:long}")]
        public IActionResult GetCustomerWithNeighbors(long customerid, [FromQuery] int high = 0, [FromQuery] int low = 0)
        {
            var leaderboard = GetLeaderboard();
            int idx = leaderboard.FindIndex(c => c.CustomerId == customerid);
            if (idx == -1)
                return NotFound($"CustomerId {customerid} not found or not ranked.");

            int start = System.Math.Max(0, idx - high);
            int end = System.Math.Min(leaderboard.Count - 1, idx + low);

            var result = leaderboard.GetRange(start, end - start + 1);

            return Ok(result);
        }
    }
}