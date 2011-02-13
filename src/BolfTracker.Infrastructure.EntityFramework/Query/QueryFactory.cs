using System.Collections.Generic;

using BolfTracker.Infrastructure.EntityFramework.Query;
using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class QueryFactory : IQueryFactory
    {
        public QueryFactory(bool caseSensitive, bool useCompiled)
        {
            CaseSensitive = caseSensitive;
            UseCompiled = useCompiled;
        }

        public bool UseCompiled
        {
            get;
            private set;
        }

        public bool CaseSensitive
        {
            get;
            private set;
        }

        public IQuery<Player> CreatePlayerByNameQuery(string name)
        {
            return new PlayerByNameQuery(UseCompiled, name);
        }

        public IQuery<IEnumerable<Game>> CreateGamesByMonthAndYearQuery(int month, int year)
        {
            return new GamesByMonthAndYearQuery(UseCompiled, month, year);
        }

        public IQuery<IEnumerable<Ranking>> CreateRankingsByMonthAndYearQuery(int month, int year)
        {
            return new RankingsByMonthAndYearQuery(UseCompiled, month, year);
        }
    }
}