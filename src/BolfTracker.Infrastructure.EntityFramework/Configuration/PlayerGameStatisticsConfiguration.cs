using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerGameStatisticsConfiguration : EntityTypeConfiguration<PlayerGameStatistics>
    {
        public PlayerGameStatisticsConfiguration()
        {
            HasKey(pgs => pgs.Id);

            Property(pgs => pgs.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(pgs => pgs.ShootingPercentage).HasPrecision(18, 3);

            HasRequired(pgs => pgs.Game).WithMany(g => g.PlayerGameStatistics).Map(pgs => pgs.MapKey("GameId"));
            HasRequired(pgs => pgs.Player).WithMany(p => p.PlayerGameStatistics).Map(pgs => pgs.MapKey("PlayerId"));

            ToTable("PlayerGameStatistics");
        }
    }
}
