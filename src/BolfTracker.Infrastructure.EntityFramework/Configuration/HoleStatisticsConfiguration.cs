using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleStatisticsConfiguration : EntityTypeConfiguration<HoleStatistics>
    {
        public HoleStatisticsConfiguration()
        {
            HasKey(hs => hs.Id);

            Property(hs => hs.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(hs => hs.ShootingPercentage).HasPrecision(18, 3);

            HasRequired(hs => hs.Hole).WithMany(h => h.HoleStatistics).Map(hs => hs.MapKey("HoleId"));

            ToTable("HoleStatistics");
        }
    }
}