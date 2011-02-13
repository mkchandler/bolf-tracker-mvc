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
    }
}