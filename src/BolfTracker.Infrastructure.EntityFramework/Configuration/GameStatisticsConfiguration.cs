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

            Property(gs => gs.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(gs => gs.Game).WithMany(g => g.GameStatistics).Map(gs => gs.MapKey("GameId"));

            ToTable("GameStatistics");
        }
    }
}