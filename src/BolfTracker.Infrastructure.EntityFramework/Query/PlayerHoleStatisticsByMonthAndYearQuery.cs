using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class PlayerHoleStatisticsByMonthAndYearQuery : QueryBase<IEnumerable<PlayerHoleStatistics>>
    {
        private static readonly Expression<Func<Database, int, int, IQueryable<PlayerHoleStatistics>>> _expression = (database, month, year) => database.PlayerHoleStatistics.Where(phs => phs.Month == month && phs.Year == year);
        private static readonly Func<Database, int, int, IQueryable<PlayerHoleStatistics>> _plainQuery = _expression.Compile();
        private static readonly Func<Database, int, int, IQueryable<PlayerHoleStatistics>> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly int _month;
        private readonly int _year;

        public PlayerHoleStatisticsByMonthAndYearQuery(bool useCompiled, int month, int year)
            : base(useCompiled)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            _month = month;
            _year = year;
        }

        public override IEnumerable<PlayerHoleStatistics> Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _month, _year) :
                   _plainQuery(database, _month, _year);
        }
    }
}
