using System.Collections.Concurrent;

namespace BootCamp.Data
{
    public static class LeaderboardData
    {
        // Thread-safe dictionary to store leaderboard data (customerId -> score)
        public static readonly ConcurrentDictionary<long, decimal> Leaderboard = new();
    }
}
