using System.Data.Entity;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class BolfTrackerContext : DbContext
    {
        DbSet<Game> Games { get; set; }
        DbSet<GameStatistics> GameStatistics { get; set; }
        DbSet<Hole> Holes { get; set; }
        DbSet<HoleStatistics> HoleStatistics { get; set; }
        DbSet<PlayerCareerStatistics> PlayerCareerStatistics { get; set; }
        DbSet<Player> Players { get; set; }
        DbSet<PlayerGameStatistics> PlayerGameStatistics { get; set; }
        DbSet<PlayerHoleStatistics> PlayerHoleStatistics { get; set; }
        DbSet<PlayerStatistics> PlayerStatistics { get; set; }
        DbSet<Ranking> Rankings { get; set; }
        DbSet<Shot> Shots { get; set; }
        DbSet<ShotType> ShotTypes { get; set; }

        public BolfTrackerContext()
            : base("")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new GameStatisticsConfiguration());
            modelBuilder.Configurations.Add(new HoleConfiguration());
            modelBuilder.Configurations.Add(new HoleStatisticsConfiguration());
            modelBuilder.Configurations.Add(new PlayerCareerStatisticsConfiguration());
            modelBuilder.Configurations.Add(new PlayerConfiguration());
            modelBuilder.Configurations.Add(new PlayerGameStatisticsConfiguration());
            modelBuilder.Configurations.Add(new PlayerHoleStatisticsConfiguration());
            modelBuilder.Configurations.Add(new PlayerStatisticsConfiguration());
            modelBuilder.Configurations.Add(new RankingConfiguration());
            modelBuilder.Configurations.Add(new ShotConfiguration());
            modelBuilder.Configurations.Add(new ShotTypeConfiguration());
        }
    }
}
