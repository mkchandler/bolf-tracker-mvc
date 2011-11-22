using System.Collections.Generic;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public interface IQueryFactory
    {
        bool UseCompiled
        {
            get;
        }

        IQuery<Player> CreatePlayerByNameQuery(string name);

        IQuery<IEnumerable<Game>> CreateGamesByMonthAndYearQuery(int month, int year);

        IQuery<IEnumerable<Ranking>> CreateRankingsByMonthAndYearQuery(int month, int year);

        IQuery<PlayerStatistics> CreatePlayerStatisticsByPlayerMonthAndYearQuery(int playerId, int month, int year);

        IQuery<IEnumerable<PlayerStatistics>> CreatePlayerStatisticsByPlayerQuery(int playerId);

        IQuery<IEnumerable<PlayerStatistics>> CreatePlayerStatisticsByMonthAndYearQuery(int month, int year);

        IQuery<IEnumerable<HoleStatistics>> CreateHoleStatisticsByMonthAndYearQuery(int month, int year);

        IQuery<PlayerHoleStatistics> CreatePlayerHoleStatisticsByPlayerHoleMonthAndYearQuery(int playerId, int holeId, int month, int year);

        IQuery<IEnumerable<PlayerHoleStatistics>> CreatePlayerHoleStatisticsByPlayerMonthAndYearQuery(int playerId, int month, int year);

        IQuery<IEnumerable<PlayerHoleStatistics>> CreatePlayerHoleStatisticsByMonthAndYearQuery(int month, int year);
    }
}