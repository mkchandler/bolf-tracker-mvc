using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class PlayerStatisticsByPlayerAndMonthQuery : QueryBase<PlayerStatistics>
    {
        private static readonly Expression<Func<Database, int, int, int, PlayerStatistics>> _expression = (database, playerId, month, year) => database.PlayerStatistics.Single(ps => ps.Player.Id == playerId && ps.Month == month && ps.Year == year);
        private static readonly Func<Database, int, int, int, PlayerStatistics> _plainQuery = _expression.Compile();
        private static readonly Func<Database, int, int, int, PlayerStatistics> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly int _playerId;
        private readonly int _month;
        private readonly int _year;

        public PlayerStatisticsByPlayerAndMonthQuery(bool useCompiled, int playerId, int month, int year)
            : base(useCompiled)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            _playerId = playerId;
            _month = month;
            _year = year;
        }

        public override PlayerStatistics Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _playerId, _month, _year) :
                   _plainQuery(database, _playerId, _month, _year);
        }
    }
}