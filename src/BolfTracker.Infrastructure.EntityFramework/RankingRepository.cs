using System.Collections.Generic;

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
            IQuery<IEnumerable<Ranking>> query = QueryFactory.CreateRankingsByMonthAndYearQuery(month, year);

            return query.Execute(Database);
        }
    }
}