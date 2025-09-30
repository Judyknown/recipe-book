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

            // �������ݣ���ɾ��
            _svc.DebugInject(200, 20);
            _svc.DebugInject(100, 90);
            _svc.DebugInject(50, 90);
            _svc.DebugInject(10, 100);
            _svc.DebugInject(98546, 200);
            _svc.DebugInject(2, 4);
        }

        // GET /leaderboard?start=1&end=3
        [HttpGet]
        public IActionResult GetRange([FromQuery] int start, [FromQuery] int end)
        {
            // ����У��
            if (start < 1 || end < start)
                return BadRequest(new ErrorDto("BadRange", "start>=1 and end>=start"));

            var list = _svc.GetRange(start, end);

            // �������Χ��û�����ݣ�������ʾ��Ϣ
            if (list == null || list.Count == 0)
            {
                return Ok(new { message = "��ǰ���а�û�пͻ����ݡ�" });
            }

            return Ok(list);
        }
    }
}
