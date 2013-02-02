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
            context.ShotTypes.AddOrUpdate(
              new ShotType { Id = 1, Name = "Make", Description = "Player made the shot" },
              new ShotType { Id = 2, Name = "Miss", Description = "Player missed the shot" },
              new ShotType { Id = 3, Name = "Push", Description = "A player pushes the points to the next hole" },
              new ShotType { Id = 4, Name = "Steal", Description = "A player steals the points from another player" },
              new ShotType { Id = 5, Name = "Sugar-Free Steal", Description = "A player steals the points when the hole is already pushed" }
            );
        }
    }
}
