using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class PlayerGameStatisticsConfiguration : EntityTypeConfiguration<PlayerGameStatistics>
    {
        public PlayerGameStatisticsConfiguration()
        {
            HasKey(pgs => pgs.Id);

            Property(pgs => pgs.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(pgs => pgs.Game).WithMany(g => g.PlayerGameStatistics);
            HasRequired(pgs => pgs.Player).WithMany(p => p.PlayerGameStatistics);

            ToTable("PlayerGameStatistics");
        }
    }
}