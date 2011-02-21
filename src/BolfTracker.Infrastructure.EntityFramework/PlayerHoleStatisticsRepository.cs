using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerHoleStatisticsRepository : RepositoryBase<PlayerHoleStatistics>, IPlayerHoleStatisticsRepository
    {
        public PlayerHoleStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public PlayerHoleStatistics GetByPlayerHoleMonthAndYear(int playerId, int holeId, int month, int year)
        {
            IQuery<PlayerHoleStatistics> query = QueryFactory.CreatePlayerHoleStatisticsByPlayerHoleMonthAndYearQuery(playerId, holeId, month, year);

            return query.Execute(Database);
        }

        public IEnumerable<PlayerHoleStatistics> GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            IQuery<IEnumerable<PlayerHoleStatistics>> query = QueryFactory.CreatePlayerHoleStatisticsByPlayerMonthAndYearQuery(playerId, month, year);

            return query.Execute(Database);
        }

        public IEnumerable<PlayerHoleStatistics> GetByMonthAndYear(int month, int year)
        {
            throw new System.NotImplementedException();
        }
    }
}