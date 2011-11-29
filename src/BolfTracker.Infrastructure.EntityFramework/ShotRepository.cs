using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class ShotRepository : RepositoryBase<Shot>, IShotRepository
    {
        public ShotRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<Shot> GetByGame(int gameId)
        {
            var shots = Database.Shots.Include(shot => shot.ShotType).Include(shot => shot.Hole).Include(shot => shot.Player).Where(shot => shot.Game.Id == gameId).ToList();

            return shots;
        }

        public IEnumerable<Shot> GetByMonthAndYear(int month, int year)
        {
            var shots = Database.Shots.Include(shot => shot.ShotType).Include(shot => shot.Hole).Where(shot => shot.Game.Date.Month == month && shot.Game.Date.Year == year).ToList();

            return shots;
        }

        public IEnumerable<Shot> GetByGameAndPlayer(int gameId, int playerId)
        {
            var shots = Database.Shots.Include(shot => shot.ShotType).Include(shot => shot.Hole).Where(shot => shot.Game.Id == gameId && shot.Player.Id == playerId).ToList();

            return shots;
        }
    }
}