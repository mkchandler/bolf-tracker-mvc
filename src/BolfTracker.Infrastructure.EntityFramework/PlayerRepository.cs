using System.Collections.Generic;
using System.Data;
using System.Linq;

using BolfTracker.Models;
using BolfTracker.Repositories;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerRepository : IPlayerRepository
    {
        public Player GetById(int id)
        {
            using (var context = new BolfTrackerContext())
            {
                var ranking = context.Players.SingleOrDefault(r => r.Id == id);

                return ranking;
            }
        }

        public Player GetByName(string name)
        {
            using (var context = new BolfTrackerContext())
            {
                var player = context.Players.First(p => p.Name == name);

                return player;
            }
        }

        public IEnumerable<Player> GetByGame(int gameId)
        {
            using (var context = new BolfTrackerContext())
            {
                var players = context.Shots.Where(shot => shot.Game.Id == gameId).Select(shot => shot.Player).Distinct().ToList();

                return players;
            }
        }

        public IEnumerable<Player> GetActiveByMonthAndYear(int month, int year)
        {
            using (var context = new BolfTrackerContext())
            {
                var players = (from pgs in context.PlayerGameStatistics
                               where pgs.Game.Date.Month == month
                                  && pgs.Game.Date.Year == year
                               select pgs.Player).Distinct();

                return players.ToList();
            }
        }

        public IEnumerable<Player> All()
        {
            using (var context = new BolfTrackerContext())
            {
                return context.Players.ToList();
            }
        }

        public void Add(Player model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Players.Attach(model);
                context.Entry(model).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public void Update(Player player)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Entry<Player>(player).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void Delete(Player model)
        {
            using (var context = new BolfTrackerContext())
            {
                context.Players.Remove(model);
                context.SaveChanges();
            }
        }
    }
}
