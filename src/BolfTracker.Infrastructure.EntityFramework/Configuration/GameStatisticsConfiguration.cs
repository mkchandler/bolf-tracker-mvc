using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class GameStatisticsConfiguration : EntityTypeConfiguration<GameStatistics>
    {
        public GameStatisticsConfiguration()
        {
            HasKey(gs => gs.Id);

            Property(gs => gs.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(gs => gs.Game).WithMany(g => g.GameStatistics);
            HasRequired(gs => gs.Player).WithMany(p => p.GameStatistics);

            ToTable("GameStatistics");
        }
    }
}