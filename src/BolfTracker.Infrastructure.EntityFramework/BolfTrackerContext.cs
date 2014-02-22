using System.Configuration;
using System.Data.Entity;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class BolfTrackerContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameStatistics> GameStatistics { get; set; }
        public DbSet<Hole> Holes { get; set; }
        public DbSet<HoleStatistics> HoleStatistics { get; set; }
        public DbSet<PlayerCareerStatistics> PlayerCareerStatistics { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerGameStatistics> PlayerGameStatistics { get; set; }
        public DbSet<PlayerHoleStatistics> PlayerHoleStatistics { get; set; }
        public DbSet<PlayerRivalryStatistics> PlayerRivalryStatistics { get; set; }
        public DbSet<PlayerStatistics> PlayerStatistics { get; set; }
        public DbSet<Ranking> Rankings { get; set; }
        public DbSet<Shot> Shots { get; set; }
        public DbSet<ShotType> ShotTypes { get; set; }

        public DbSet<PlayerBadges> PlayerBadges { get; set; }
        public BolfTrackerContext() : base(ConfigurationManager.ConnectionStrings["BolfTracker"].ConnectionString)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add<Game>(new GameConfiguration());
            modelBuilder.Configurations.Add<GameStatistics>(new GameStatisticsConfiguration());
            modelBuilder.Configurations.Add<Hole>(new HoleConfiguration());
            modelBuilder.Configurations.Add<HoleStatistics>(new HoleStatisticsConfiguration());
            modelBuilder.Configurations.Add<PlayerCareerStatistics>(new PlayerCareerStatisticsConfiguration());
            modelBuilder.Configurations.Add<Player>(new PlayerConfiguration());
            modelBuilder.Configurations.Add<PlayerGameStatistics>(new PlayerGameStatisticsConfiguration());
            modelBuilder.Configurations.Add<PlayerHoleStatistics>(new PlayerHoleStatisticsConfiguration());
            modelBuilder.Configurations.Add<PlayerRivalryStatistics>(new PlayerRivalryStatisticsConfiguration());
            modelBuilder.Configurations.Add<PlayerStatistics>(new PlayerStatisticsConfiguration());
            modelBuilder.Configurations.Add<Ranking>(new RankingConfiguration());
            modelBuilder.Configurations.Add<Shot>(new ShotConfiguration());
            modelBuilder.Configurations.Add<ShotType>(new ShotTypeConfiguration());
            modelBuilder.Configurations.Add<PlayerBadges>(new PlayerBadgesConfiguration());
        }
    }
}
