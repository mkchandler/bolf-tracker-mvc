using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class PlayerHoleStatisticsByPlayerMonthAndYearQuery : QueryBase<IEnumerable<PlayerHoleStatistics>>
    {
        private static readonly Expression<Func<Database, int, int, int, IQueryable<PlayerHoleStatistics>>> _expression = (database, playerId, month, year) => database.PlayerHoleStatistics.Where(phs => phs.Player.Id == playerId && phs.Month == month && phs.Year == year);
        private static readonly Func<Database, int, int, int, IQueryable<PlayerHoleStatistics>> _plainQuery = _expression.Compile();
        private static readonly Func<Database, int, int, int, IQueryable<PlayerHoleStatistics>> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly int _playerId;
        private readonly int _month;
        private readonly int _year;

        public PlayerHoleStatisticsByPlayerMonthAndYearQuery(bool useCompiled, int playerId, int month, int year)
            : base(useCompiled)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            _playerId = playerId;
            _month = month;
            _year = year;
        }

        public override IEnumerable<PlayerHoleStatistics> Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _playerId, _month, _year) :
                   _plainQuery(database, _playerId, _month, _year);
        }
    }
}