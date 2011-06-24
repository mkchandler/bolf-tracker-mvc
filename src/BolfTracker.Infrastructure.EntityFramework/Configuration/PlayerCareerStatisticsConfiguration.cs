using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class PlayerCareerStatisticsConfiguration : EntityTypeConfiguration<PlayerCareerStatistics>
    {
        public PlayerCareerStatisticsConfiguration()
        {
            HasKey(pcs => pcs.Id);

            Property(pcs => pcs.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(pcs => pcs.Player).WithMany(p => p.PlayerCareerStatistics);

            ToTable("PlayerCareerStatistics");
        }
    }
}