using System.Collections.Generic;
using System.Linq;

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
            var holeStatistics = Database.HoleStatistics.Where(hs => hs.Month == month && hs.Year == year).ToList();

            return holeStatistics;
        }
    }
}