using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

using Dapper;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class ShotRepository : IShotRepository
    {
        public Shot GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var shot = context.Shots.SingleOrDefault(s => s.Id == id);

                return shot;
            }
        }

        public IEnumerable<Shot> GetByGame(int gameId)
        {
            using (var context = new BolfTrackerContext())
            {
                var shots = context.Shots.Include(shot => shot.ShotType).Include(shot => shot.Hole).Include(shot => shot.Player).Include(shot => shot.Game).Where(shot => shot.Game.Id == gameId).ToList();

                return shots;
            }
        }

        public IEnumerable<Shot> GetByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var shots = context.Shots.Include(shot => shot.ShotType).Include(shot => shot.Hole).Include(shot => shot.Player).Include(shot => shot.Game).Where(shot => shot.Game.Date.Month == month && shot.Game.Date.Year == year).ToList();

                return shots;
            }
        }

        public IEnumerable<Shot> GetByGameAndPlayer(int gameId, int playerId)
        {
            using (var context = new BolfTrackerContext())
            {
                var shots = context.Shots.Include(shot => shot.ShotType).Include(shot => shot.Hole).Include(shot => shot.Player).Include(shot => shot.Game).Where(shot => shot.Game.Id == gameId && shot.Player.Id == playerId).ToList();

                return shots;
            }
        }

        public IEnumerable<Shot> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.Shots.ToList();
            }
        }

        public void Add(Shot shot)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Shots.Attach(shot);
                context.Entry(shot).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Shot shot)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Shots.Attach(shot);
                context.Entry<Shot>(shot).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            using (var connection = BolfTrackerDbConnection.GetProfiledConnection())
            {
                connection.Open();

                connection.Execute("DELETE FROM Shot WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
