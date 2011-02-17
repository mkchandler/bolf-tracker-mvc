using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameStatisticsRepository : RepositoryBase<GameStatistics>, IGameStatisticsRepository
    {
        public GameStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<GameStatistics> GetByPlayerAndMonth(int playerId, int month, int year)
        {
            IQuery<IEnumerable<GameStatistics>> query = QueryFactory.CreateGameStatisticsByPlayerAndMonthQuery(playerId, month, year);

            return query.Execute(Database);
        }
    }
}