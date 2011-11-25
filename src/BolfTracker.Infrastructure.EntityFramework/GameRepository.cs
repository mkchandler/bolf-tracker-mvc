using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameRepository: RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<Game> GetByMonthAndYear(int month, int year)
        {
            var games = Database.Games.Where(game => game.Date.Month == month && game.Date.Year == year).ToList();

            return games;
        }

        public IEnumerable<Game> GetByMonthAndYearWithStatistics(int month, int year)
        {
            // TODO: This query is not very effecient and could probably be written better
            var games = Database.Games.Include(game => game.GameStatistics).Include(game => game.PlayerGameStatistics.Select(pgs => pgs.Player)).Where(game => game.Date.Month == month && game.Date.Year == year).ToList();

            return games;
        }
    }
}