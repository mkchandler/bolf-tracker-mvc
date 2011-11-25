using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerGameStatisticsRepository : RepositoryBase<PlayerGameStatistics>, IPlayerGameStatisticsRepository
    {
        public PlayerGameStatisticsRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayer(int playerId)
        {
            var playerGameStatistics = Database.PlayerGameStatistics.Where(pgs => pgs.Player.Id == playerId).ToList();

            return playerGameStatistics;
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            var playerGameStatistics = Database.PlayerGameStatistics.Where(pgs => pgs.Player.Id == playerId && pgs.Game.Date.Month == month && pgs.Game.Date.Year == year).ToList();

            return playerGameStatistics;
        }
    }
}