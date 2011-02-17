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

        public IQuery<PlayerStatistics> CreatePlayerStatisticsByPlayerAndMonthQuery(int playerId, int month, int year)
        {
            return new PlayerStatisticsByPlayerAndMonthQuery(UseCompiled, playerId, month, year);
        }

        public IQuery<IEnumerable<PlayerStatistics>> CreatePlayerStatisticsByPlayerQuery(int playerId)
        {
            return new PlayerStatisticsByPlayerQuery(UseCompiled, playerId);
        }

        public IQuery<IEnumerable<PlayerStatistics>> CreatePlayerStatisticsByMonthAndYearQuery(int month, int year)
        {
            return new PlayerStatisticsByMonthAndYearQuery(UseCompiled, month, year);
        }

        public IQuery<IEnumerable<GameStatistics>> CreateGameStatisticsByPlayerAndMonthQuery(int playerId, int month, int year)
        {
            return new GameStatisticsByPlayerAndMonthQuery(UseCompiled, playerId, month, year);
        }
    }
}