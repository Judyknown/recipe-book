using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BootCamp.Data;

namespace BootCamp
{
    public sealed class LeaderboardService
    {
        // A light lock only used to create consistent snapshots (enumeration of ConcurrentDictionary is safe but not atomic)
        private readonly ReaderWriterLockSlim _snapshotLock = new(LockRecursionPolicy.NoRecursion);

        // Add delta to a customer's score (insert if absent). Returns new accumulated score.
        public decimal UpdateScore(long customerId, decimal delta)
        {
            return LeaderboardData.Leaderboard.AddOrUpdate(
                customerId,
                delta,
                (_, old) => old + delta);
        }

        // Build a stable ordered snapshot (score desc, customerId asc) of items with score > 0.
        private List<RankItem> BuildSnapshot()
        {
            _snapshotLock.EnterReadLock();
            try
            {
                return LeaderboardData.Leaderboard
                    .Where(kv => kv.Value > 0m)
                    .OrderByDescending(kv => kv.Value)
                    .ThenBy(kv => kv.Key)
                    .Select((kv, index) => new RankItem(kv.Key, kv.Value, index + 1))
                    .ToList();
            }
            finally
            {
                _snapshotLock.ExitReadLock();
            }
        }

        // Get rank range [start, end] (1-based inclusive)
        public List<RankItem> GetRange(int start, int end)
        {
            if (start < 1 || end < start) return new();
            var snapshot = BuildSnapshot();
            if (start > snapshot.Count) return new();
            int take = end - start + 1;
            return snapshot.Skip(start - 1).Take(take).ToList();
        }

        // Get a customer and its high(low) neighbors
        public CustomerWithNeighborsResult? GetWithNeighbors(long customerId, int high, int low)
        {
            if (high < 0) high = 0;
            if (low < 0) low = 0;

            var snapshot = BuildSnapshot();
            var idx = snapshot.FindIndex(r => r.CustomerId == customerId);
            if (idx < 0) return null;

            int highStart = idx - high;
            if (highStart < 0) highStart = 0;
            var highNeighbors = snapshot.Skip(highStart).Take(idx - highStart).ToList();
            var lowNeighbors = snapshot.Skip(idx + 1).Take(low).ToList();

            return new CustomerWithNeighborsResult(snapshot[idx], highNeighbors, lowNeighbors);
        }

        // Debug seed (overwrite absolute score if >0)
        public void DebugInject(long customerId, decimal score)
        {
            if (score <= 0m) return;
            LeaderboardData.Leaderboard.AddOrUpdate(customerId, score, (_, _) => score);
        }
    }

    public readonly record struct RankItem(long CustomerId, decimal Score, int Rank);
    public readonly record struct ErrorDto(string Code, string Message);
    public sealed record CustomerWithNeighborsResult(RankItem Customer, IReadOnlyList<RankItem> High, IReadOnlyList<RankItem> Low);
}