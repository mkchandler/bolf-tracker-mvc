using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerStatisticsRepository : RepositoryBase<PlayerStatistics>, IPlayerStatisticsRepository
    {
        public PlayerStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public PlayerStatistics GetByPlayerMonthAndYear(int playerId, int month, int year)
        {
            IQuery<PlayerStatistics> query = QueryFactory.CreatePlayerStatisticsByPlayerMonthAndYearQuery(playerId, month, year);

            return query.Execute(Database);
        }

        public IEnumerable<PlayerStatistics> GetByPlayer(int playerId)
        {
            IQuery<IEnumerable<PlayerStatistics>> query = QueryFactory.CreatePlayerStatisticsByPlayerQuery(playerId);

            return query.Execute(Database);
        }

        public IEnumerable<PlayerStatistics> GetByMonthAndYear(int month, int year)
        {
            IQuery<IEnumerable<PlayerStatistics>> query = QueryFactory.CreatePlayerStatisticsByMonthAndYearQuery(month, year);

            return query.Execute(Database);
        }
    }
}