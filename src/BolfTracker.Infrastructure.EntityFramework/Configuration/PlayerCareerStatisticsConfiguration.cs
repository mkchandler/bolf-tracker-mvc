using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerCareerStatisticsConfiguration : EntityTypeConfiguration<PlayerCareerStatistics>
    {
        public PlayerCareerStatisticsConfiguration()
        {
            HasKey(pcs => pcs.Id);

            Property(pcs => pcs.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(pcs => pcs.ShootingPercentage).HasPrecision(18, 3);
            Property(pcs => pcs.WinningPercentage).HasPrecision(18, 3);
            Property(pcs => pcs.StainlessStealsPerGame).HasPrecision(18, 1);
            Property(pcs => pcs.PointsPerGame).HasPrecision(18, 1);
            Property(pcs => pcs.PushesPerGame).HasPrecision(18, 1);
            Property(pcs => pcs.StealsPerGame).HasPrecision(18, 1);
            Property(pcs => pcs.SugarFreeStealsPerGame).HasPrecision(18, 1);

            HasRequired(pcs => pcs.Player).WithMany(p => p.PlayerCareerStatistics).Map(pgs => pgs.MapKey("PlayerId"));

            ToTable("PlayerCareerStatistics");
        }
    }
}
