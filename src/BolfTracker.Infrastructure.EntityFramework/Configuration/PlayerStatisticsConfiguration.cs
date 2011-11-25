using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class PlayerStatisticsConfiguration : EntityTypeConfiguration<PlayerStatistics>
    {
        public PlayerStatisticsConfiguration()
        {
            HasKey(ps => ps.Id);

            Property(ps => ps.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(ps => ps.Player).WithMany(p => p.PlayerStatistics).Map(ps => ps.MapKey("PlayerId"));

            Ignore(ps => ps.TotalGames);
            Ignore(ps => ps.NormalSteals);
            Ignore(ps => ps.NormalStealsPerGame);

            ToTable("PlayerStatistics");
        }
    }
}