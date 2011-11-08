using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<Player> GetByGame(int gameId)
        {
            var players = Database.Shots.Where(shot => shot.Game.Id == gameId).Select(shot => shot.Player).Distinct().ToList();

            return players;
        }

        public IEnumerable<Player> GetActiveByMonthAndYear(int month, int year)
        {
            var players = Database.Shots.Where(shot => shot.Game.Date.Month == month && shot.Game.Date.Year == year).Select(shot => shot.Player).Distinct().ToList();

            return players;
        }
    }
}