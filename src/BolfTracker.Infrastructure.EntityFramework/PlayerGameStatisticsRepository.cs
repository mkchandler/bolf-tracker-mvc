using System;
using System.Collections.Generic;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerGameStatisticsRepository : RepositoryBase<PlayerGameStatistics>, IPlayerGameStatisticsRepository
    {
        public PlayerGameStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayer(int playerId)
        {
            var query = Database.PlayerGameStatistics.Where(pgs => pgs.Player.Id == playerId);

            return query;
        }

        public IEnumerable<PlayerGameStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            var query = Database.PlayerGameStatistics.Where(pgs => pgs.Player.Id == playerId && pgs.Game.Date.Month == month && pgs.Game.Date.Year == year);

            return query;
        }
    }
}