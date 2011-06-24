using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerCareerStatisticsRepository : RepositoryBase<PlayerCareerStatistics>, IPlayerCareerStatisticsRepository
    {
        public PlayerCareerStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public PlayerCareerStatistics GetByPlayer(int playerId)
        {
            var query = Database.PlayerCareerStatistics.Where(pcs => pcs.Player.Id == playerId).Single();

            return query;
        }
    }
}