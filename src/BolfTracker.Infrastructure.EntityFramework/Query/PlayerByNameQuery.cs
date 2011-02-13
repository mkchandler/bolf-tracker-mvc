using System;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Query
{
    public class PlayerByNameQuery : QueryBase<Player>
    {
        private static readonly Expression<Func<Database, string, Player>> _expression = (database, name) => database.Players.SingleOrDefault(player => player.Name == name);
        private static readonly Func<Database, string, Player> _plainQuery = _expression.Compile();
        private static readonly Func<Database, string, Player> _compiledQuery = CompiledQuery.Compile(_expression);

        private readonly string _name;

        public PlayerByNameQuery(bool useCompiled, string name) : base(useCompiled)
        {
            Check.Argument.IsNotNullOrEmpty(name, "name", "Player name must contain a value");

            _name = name;
        }

        public override Player Execute(Database database)
        {
            Check.Argument.IsNotNull(database, "database");

            return UseCompiled ?
                   _compiledQuery(database, _name) :
                   _plainQuery(database, _name);
        }
    }
}