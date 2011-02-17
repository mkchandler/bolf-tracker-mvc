using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class PlayerStatisticsByPlayerQuery : QueryBase<IEnumerable<PlayerStatistics>>
    {
        private static readonly Expression<Func<Database, int, IQueryable<PlayerStatistics>>> _expression = (database, playerId) => database.PlayerStatistics.Where(ps => ps.Player.Id == playerId);
        private static readonly Func<Database, int, IQueryable<PlayerStatistics>> _plainQuery = _expression.Compile();
        private static readonly Func<Database, int, IQueryable<PlayerStatistics>> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly int _playerId;

        public PlayerStatisticsByPlayerQuery(bool useCompiled, int playerId)
            : base(useCompiled)
        {
            Check.Argument.IsNotZeroOrNegative(playerId, "playerId");

            _playerId = playerId;
        }

        public override IEnumerable<PlayerStatistics> Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _playerId) :
                   _plainQuery(database, _playerId);
        }
    }
}