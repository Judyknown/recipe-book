# My Recipe Book / BootCamp Demo

This repository currently contains a .NET 8 based sample Leaderboard Web API (in the `BootCamp` directory). It demonstrates simple score updates and leaderboard range queries. The original Recipe Book idea can be merged / expanded later.

## Tech Stack
- .NET 8 (ASP.NET Core)
- Swagger / OpenAPI (`Swashbuckle.AspNetCore`)
- Thread-safe in?memory data structure + read lock to build consistent snapshots

## Quick Start
```bash
# Restore & build
dotnet restore
dotnet build
# Run Web API (default https://localhost:7280 or similar)
dotnet run --project BootCamp/BootCamp.csproj
```
After starting, open Swagger UI: `https://localhost:<port>/swagger` to explore and test endpoints.

## Core Endpoints
| Feature | Method & Route | Description |
|---------|----------------|-------------|
| Update (increment) a customer's score | POST `/customer/{customerid}/score/{score}` | `score` is a delta in range [-1000, 1000]; returns new accumulated score (creates entry if absent) |
| Query a leaderboard range | GET `/leaderboard?start={s}&end={e}` | 1-based inclusive rank range; returns a sorted slice (Score DESC, CustomerId ASC) |

Leaderboard item shape:
```json
{
  "customerId": 123,
  "score": 456.78,
  "rank": 1
}
```
Error example:
```json
{ "code": "BadRange", "message": "start>=1 and end>=start" }
```

## Example Requests
```bash
# Add +50 to user 101
curl -X POST https://localhost:7280/customer/101/score/50

# Add another +20
curl -X POST https://localhost:7280/customer/101/score/20

# Query top 10
curl "https://localhost:7280/leaderboard?start=1&end=10"
```

## Implementation Notes
- `LeaderboardService` uses `ConcurrentDictionary<long, decimal>` for (CustomerId -> Score)
- Read lock around snapshot building gives a consistent ordered view during enumeration
- Sorting rule: Score DESC, CustomerId ASC
- Only scores > 0 appear on the leaderboard

## Directory Structure (excerpt)
```
BootCamp/
  Controllers/
    CustomerController.cs
    LeaderboardRankController.cs
  LeaderboardService.cs
  Program.cs
README.md
```

## Future / Extension Ideas
- Expose endpoint to get a user with high/low neighbors (`GetWithNeighbors` already exists in the service layer)
- Add reset / bulk import endpoints
- Introduce persistence (PostgreSQL / Redis) instead of in?memory storage
- Add authentication / authorization (API Key / JWT)
- Unit tests for snapshot ordering, range slicing, concurrency integrity

## Testing Strategy
- Concurrent score updates (multi-thread) ? verify final accumulated scores & absence of race issues
- Random data generation to validate deterministic ordering (stable sort)
- Edge cases: empty leaderboard / start > count / end < start / start=1

## License
Not yet specified (can add MIT or other as needed).

## Recent Changelog
- Added leaderboard range query controller
- Added score update endpoint & base service logic
- Expanded README (English version)

Contributions via Issues / PRs are welcome.

