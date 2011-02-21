using System;
using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleStatisticsRepository : RepositoryBase<HoleStatistics>, IHoleStatisticsRepository
    {
        public HoleStatisticsRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<HoleStatistics> GetByMonthAndYear(int month, int year)
        {
            IQuery<IEnumerable<HoleStatistics>> query = QueryFactory.CreateHoleStatisticsByMonthAndYearQuery(month, year);

            return query.Execute(Database);
        }
    }
}