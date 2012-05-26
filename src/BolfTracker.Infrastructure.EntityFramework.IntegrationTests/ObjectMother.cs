using System;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.IntegrationTests
{
    public static class ObjectMother
    {
        public static Player CreatePlayer()
        {
            return new Player { Name = Random() };
        }

        public static Game CreateGame()
        {
            return new Game { Date = DateTime.Today };
        }

        public static Hole CreateHole(int id)
        {
            return new Hole { Id = id, Par = 1 };
        }

        public static ShotType CreateShotType()
        {
            return new ShotType
            {
                Id = Int32.MaxValue,
                Name = Random(),
                Description = Random()
            };
        }

        public static Shot CreateShot(Game game, Player player, ShotType shotType, Hole hole)
        {
            return new Shot
            {
                Game = game,
                Player = player,
                ShotType = shotType,
                Hole = hole,
                ShotMade = true,
                Attempts = 1,
                Points = 10
            };
        }

        public static GameStatistics CreateGameStatistics(Game game)
        {
            return new GameStatistics
            {
                Game = game,
                HoleCount = 10,
                PlayerCount = 9,
                Points = 8,
                ShotsMade = 7,
                ShotsMissed = 6,
                Attempts = 5,
                Pushes = 4,
                Steals = 3,
                SugarFreeSteals = 2,
                StainlessSteals = 1,
                OvertimeCount = 0,
                ShootingPercentage = 0.234M
            };
        }

        public static PlayerStatistics CreatePlayerStatistics(Player player)
        {
            return new PlayerStatistics
            {
                Player = player,
                Month = DateTime.Today.Month,
                Year = DateTime.Today.Year,
                Attempts = 10,
                ShotsMade = 5,
                Points = 12,
                Pushes = 3,
                Steals = 2,
                SugarFreeSteals = 1,
                Wins = 6,
                Losses = 5
            };
        }

        public static HoleStatistics CreateHoleStatistics(Hole hole)
        {
            return new HoleStatistics
            {
                Hole = hole,
                Month = DateTime.Today.Month,
                Year = DateTime.Today.Year,
                Attempts = 10,
                ShotsMade = 5,
                ShootingPercentage = .500M,
                PointsScored = 12,
                Pushes = 3,
                Steals = 2,
                SugarFreeSteals = 1
            };
        }

        public static PlayerHoleStatistics CreatePlayerHoleStatistics(Player player, Hole hole)
        {
            return new PlayerHoleStatistics
            {
                Player = player,
                Hole = hole,
                Month = DateTime.Today.Month,
                Year = DateTime.Today.Year,
                Attempts = 10,
                ShotsMade = 5,
                ShootingPercentage = .500M,
                PointsScored = 12,
                Pushes = 3,
                Steals = 2,
                SugarFreeSteals = 1
            };
        }

        public static PlayerRivalryStatistics CreatePlayerRivalryStatistics(Game game, Player player, Player affectedPlayer, Hole hole, ShotType shotType)
        {
            return new PlayerRivalryStatistics
            {
                Game = game,
                Player = player,
                AffectedPlayer = affectedPlayer,
                Hole = hole,
                ShotType = shotType,
                Attempts = 10,
                Points = 5
            };
        }

        public static Ranking CreateRanking(Player player)
        {
            return new Ranking
            {
                Month = DateTime.Today.Month,
                Year = DateTime.Today.Year,
                Player = player,
                Wins = 5,
                Losses = 7,
                TotalPoints = 54,
                GamesBack = 2,
                LastTenWins = 5,
                LastTenLosses = 5,
                LastTenWinningPercentage = .111M,
                WinningPercentage = .222M,
                PointsPerGame = 4
            };
        }

        private static string Random()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
