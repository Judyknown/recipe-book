using System.Collections.Generic;
using System.Threading;

namespace BootCamp
{
    public sealed class LeaderboardService
    {
        private readonly ReaderWriterLockSlim _lock = new(LockRecursionPolicy.NoRecursion);

        // 分数降序；同分 ID 升序
        private readonly SortedDictionary<decimal, SortedSet<long>> _board =
            new(new DescDecimalComparer());

        public List<RankItem> GetRange(int start, int end)
        {
            _lock.EnterReadLock();
            try
            {
                var res = new List<RankItem>(end - start + 1);
                int rank = 0;

                foreach (var kv in _board)          // 分数降序
                {
                    var score = kv.Key;
                    foreach (var id in kv.Value)    // 同分 ID 升序
                    {
                        rank++;
                        if (rank < start) continue;
                        if (rank > end) return res;

                        res.Add(new RankItem(id, score, rank));
                    }
                }
                return res;
            }
            finally { _lock.ExitReadLock(); }
        }

        // 给测试用的注入方法
        public void DebugInject(long customerId, decimal score)
        {
            _lock.EnterWriteLock();
            try
            {
                if (score <= 0m) return;
                if (!_board.TryGetValue(score, out var set))
                {
                    set = new SortedSet<long>();
                    _board[score] = set;
                }
                set.Add(customerId);
            }
            finally { _lock.ExitWriteLock(); }
        }
    }

    internal sealed class DescDecimalComparer : IComparer<decimal>
    {
        public int Compare(decimal x, decimal y) => -x.CompareTo(y); // 降序
    }

    public readonly record struct RankItem(long CustomerId, decimal Score, int Rank);
    public readonly record struct ErrorDto(string Code, string Message);
}