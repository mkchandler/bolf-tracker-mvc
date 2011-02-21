using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class PlayerHoleStatisticsConfiguration : EntityTypeConfiguration<PlayerHoleStatistics>
    {
        public PlayerHoleStatisticsConfiguration()
        {
            HasKey(phs => phs.Id);

            Property(phs => phs.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(ps => ps.Player).WithMany(p => p.PlayerHoleStatistics);

            ToTable("PlayerHoleStatistics");
        }
    }
}