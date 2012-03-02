using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerHoleStatisticsConfiguration : EntityTypeConfiguration<PlayerHoleStatistics>
    {
        public PlayerHoleStatisticsConfiguration()
        {
            HasKey(phs => phs.Id);

            Property(phs => phs.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(phs => phs.ShootingPercentage).HasPrecision(18, 3);

            HasRequired(ps => ps.Player).WithMany(p => p.PlayerHoleStatistics).Map(ps => ps.MapKey("PlayerId"));
            HasRequired(ps => ps.Hole).WithMany(h => h.PlayerHoleStatistics).Map(ps => ps.MapKey("HoleId"));

            ToTable("PlayerHoleStatistics");
        }
    }
}