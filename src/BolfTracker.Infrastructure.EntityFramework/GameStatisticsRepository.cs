using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameStatisticsRepository : RepositoryBase<GameStatistics>, IGameStatisticsRepository
    {
        public GameStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }
    }
}