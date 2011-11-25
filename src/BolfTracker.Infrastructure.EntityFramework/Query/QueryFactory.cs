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

        public IQuery<PlayerStatistics> CreatePlayerStatisticsByPlayerMonthAndYearQuery(int playerId, int month, int year)
        {
            return new PlayerStatisticsByPlayerMonthAndYearQuery(UseCompiled, playerId, month, year);
        }

        public IQuery<IEnumerable<PlayerStatistics>> CreatePlayerStatisticsByPlayerQuery(int playerId)
        {
            return new PlayerStatisticsByPlayerQuery(UseCompiled, playerId);
        }

        public IQuery<IEnumerable<PlayerStatistics>> CreatePlayerStatisticsByMonthAndYearQuery(int month, int year)
        {
            return new PlayerStatisticsByMonthAndYearQuery(UseCompiled, month, year);
        }

        public IQuery<PlayerHoleStatistics> CreatePlayerHoleStatisticsByPlayerHoleMonthAndYearQuery(int playerId, int holeId, int month, int year)
        {
            return new PlayerHoleStatisticsByPlayerHoleMonthAndYearQuery(UseCompiled, playerId, holeId, month, year);
        }

        public IQuery<IEnumerable<PlayerHoleStatistics>> CreatePlayerHoleStatisticsByPlayerMonthAndYearQuery(int playerId, int month, int year)
        {
            return new PlayerHoleStatisticsByPlayerMonthAndYearQuery(UseCompiled, playerId, month, year);
        }

        public IQuery<IEnumerable<PlayerHoleStatistics>> CreatePlayerHoleStatisticsByMonthAndYearQuery(int month, int year)
        {
            return new PlayerHoleStatisticsByMonthAndYearQuery(UseCompiled, month, year);
        }
    }
}