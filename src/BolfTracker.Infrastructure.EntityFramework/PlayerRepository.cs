using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(IDatabaseFactory databaseFactory, IQueryFactory queryFactory) : base(databaseFactory, queryFactory)
        {
        }

        public Player GetByName(string name)
        {
            IQuery<Player> query = QueryFactory.CreatePlayerByNameQuery(name);

            return query.Execute(Database);
        }
    }
}