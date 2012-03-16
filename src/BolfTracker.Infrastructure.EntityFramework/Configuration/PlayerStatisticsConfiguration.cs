using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerStatisticsConfiguration : EntityTypeConfiguration<PlayerStatistics>
    {
        public PlayerStatisticsConfiguration()
        {
            HasKey(ps => ps.Id);

            Property(ps => ps.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(ps => ps.ShootingPercentage).HasPrecision(18, 3);
            Property(ps => ps.WinningPercentage).HasPrecision(18, 3);
            Property(ps => ps.StainlessStealsPerGame).HasPrecision(18, 3);
            Property(ps => ps.PointsPerGame).HasPrecision(18, 1);
            Property(ps => ps.PushesPerGame).HasPrecision(18, 1);
            Property(ps => ps.StealsPerGame).HasPrecision(18, 1);
            Property(ps => ps.SugarFreeStealsPerGame).HasPrecision(18, 1);

            HasRequired(ps => ps.Player).WithMany(p => p.PlayerStatistics).Map(ps => ps.MapKey("PlayerId"));

            Ignore(ps => ps.TotalGames);
            Ignore(ps => ps.NormalSteals);
            Ignore(ps => ps.NormalStealsPerGame);

            ToTable("PlayerStatistics");
        }
    }
}
