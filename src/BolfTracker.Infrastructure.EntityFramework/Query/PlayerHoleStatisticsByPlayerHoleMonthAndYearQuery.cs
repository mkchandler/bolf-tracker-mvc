using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class PlayerHoleStatisticsByPlayerHoleMonthAndYearQuery : QueryBase<PlayerHoleStatistics>
    {
        private static readonly Expression<Func<Database, int, int, int, int, PlayerHoleStatistics>> _expression = (database, playerId, holeId, month, year) => database.PlayerHoleStatistics.Single(phs => phs.Player.Id == playerId && phs.Hole.Id == holeId && phs.Month == month && phs.Year == year);
        private static readonly Func<Database, int, int, int, int, PlayerHoleStatistics> _plainQuery = _expression.Compile();
        private static readonly Func<Database, int, int, int, int, PlayerHoleStatistics> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly int _playerId;
        private readonly int _holeId;
        private readonly int _month;
        private readonly int _year;

        public PlayerHoleStatisticsByPlayerHoleMonthAndYearQuery(bool useCompiled, int playerId, int holeId, int month, int year)
            : base(useCompiled)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(holeId, "holeId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            _playerId = playerId;
            _holeId = holeId;
            _month = month;
            _year = year;
        }

        public override PlayerHoleStatistics Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _playerId, _holeId, _month, _year) :
                   _plainQuery(database, _playerId, _holeId, _month, _year);
        }
    }
}