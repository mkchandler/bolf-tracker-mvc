using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<BolfTrackerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        // This method will be called after migrating to the latest version
        protected override void Seed(BolfTrackerContext context)
        {
            // These values will be initial constants for all of bolf tracker
            context.ShotTypes.AddOrUpdate(
              new ShotType { Id = 1, Name = "Make", Description = "Player made the shot" },
              new ShotType { Id = 2, Name = "Miss", Description = "Player missed the shot" },
              new ShotType { Id = 3, Name = "Push", Description = "A player pushes the points to the next hole" },
              new ShotType { Id = 4, Name = "Steal", Description = "A player steals the points from another player" },
              new ShotType { Id = 5, Name = "Sugar-Free Steal", Description = "A player steals the points when the hole is already pushed" }
            );

            // Set up some default players for testing purposes (will eventually be part of a test league)
            context.Players.AddOrUpdate(
              new Player { Name = "Matt" },
              new Player { Name = "Chris" },
              new Player { Name = "Aaron" },
              new Player { Name = "Bradley" },
              new Player { Name = "Jeaux" }
            );

            // Set up some default holes for testing purposes (will eventually be part of a test league)
            context.Holes.AddOrUpdate(
              new Hole { Id = 1, Par = 1 },
              new Hole { Id = 2, Par = 2 },
              new Hole { Id = 3, Par = 2 },
              new Hole { Id = 4, Par = 3 },
              new Hole { Id = 5, Par = 4 },
              new Hole { Id = 6, Par = 1 },
              new Hole { Id = 7, Par = 2 },
              new Hole { Id = 8, Par = 2 },
              new Hole { Id = 9, Par = 3 },
              new Hole { Id = 10, Par = 5 }
            );
        }
    }
}
