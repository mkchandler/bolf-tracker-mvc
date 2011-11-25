using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class RankingRepository : RepositoryBase<Ranking>, IRankingRepository
    {
        public RankingRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<Ranking> GetByMonthAndYear(int month, int year)
        {
            var rankings = Database.Rankings.Include(ranking => ranking.Player).Where(ranking => ranking.Month == month && ranking.Year == year).ToList();

            return rankings;
        }
    }
}