using System;
using System.Collections.Generic;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class GameRepository: RepositoryBase<Game>, IGameRepository
    {
        public GameRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public IEnumerable<Game> GetByMonthAndYear(int month, int year)
        {
            IQuery<IEnumerable<Game>> query = QueryFactory.CreateGamesByMonthAndYearQuery(month, year);

            return query.Execute(Database);
        }
    }
}