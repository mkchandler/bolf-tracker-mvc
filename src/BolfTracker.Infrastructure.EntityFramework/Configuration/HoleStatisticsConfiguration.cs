using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class HoleStatisticsConfiguration : EntityTypeConfiguration<HoleStatistics>
    {
        public HoleStatisticsConfiguration()
        {
            HasKey(hs => hs.Id);

            Property(hs => hs.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(hs => hs.Hole).WithMany(h => h.HoleStatistics);

            ToTable("HoleStatistics");
        }
    }
}