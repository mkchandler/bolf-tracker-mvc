using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class GamesByMonthAndYearQuery : QueryBase<IEnumerable<Game>>
    {
        private static readonly Expression<Func<Database, int, int, IQueryable<Game>>> _expression = (database, month, year) => database.Games.Where(g => g.Date.Month == month && g.Date.Year == year);
        private static readonly Func<Database, int, int, IQueryable<Game>> _plainQuery = _expression.Compile();
        private static readonly Func<Database, int, int, IQueryable<Game>> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly int _month;
        private readonly int _year;

        public GamesByMonthAndYearQuery(bool useCompiled, int month, int year)
            : base(useCompiled)
        {
            Check.Argument.IsNotZeroOrNegative(month, "month");
            Check.Argument.IsNotZeroOrNegative(year, "year");

            _month = month;
            _year = year;
        }

        public override IEnumerable<Game> Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _month, _year) :
                   _plainQuery(database, _month, _year);
        }
    }
}