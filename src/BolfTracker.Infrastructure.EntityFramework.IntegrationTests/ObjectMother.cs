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
            return new ShotType { Id = Int32.MaxValue, Name = Random(), Description = Random() };
        }

        public static Shot CreateScore()
        {
            return new Shot { Game = CreateGame(), Player = CreatePlayer(), ShotType = CreateShotType(), Attempts = 1 };
        }

        public static GameStatistics CreateGameStatistics()
        {
            return
                new GameStatistics
                {
                    Game = CreateGame(),
                    Player = CreatePlayer(),
                    Attempts = 10,
                    ShotsMade = 5,
                    Pushes = 3,
                    Steals = 2,
                    SugarFreeSteals = 1,
                    Points = 12,
                    Winner = true
                };
        }

        public static PlayerStatistics CreatePlayerStatistics()
        {
            return
                new PlayerStatistics
                {
                    Player = CreatePlayer(),
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

        public static Ranking CreateRanking()
        {
            return
                new Ranking
                {
                    Month = DateTime.Today.Month,
                    Year = DateTime.Today.Year,
                    Player = CreatePlayer(),
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