using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class PlayerRivalryStatisticsConfiguration : EntityTypeConfiguration<PlayerRivalryStatistics>
    {
        public PlayerRivalryStatisticsConfiguration()
        {
            HasKey(prs => prs.Id);

            Property(prs => prs.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(prs => prs.Game).WithMany(g => g.PlayerRivalryStatistics).Map(pgs => pgs.MapKey("GameId")).WillCascadeOnDelete(false);
            HasRequired(prs => prs.Player).WithMany(p => p.PlayerRivalryStatistics).Map(pgs => pgs.MapKey("PlayerId")).WillCascadeOnDelete(false);
            HasRequired(prs => prs.AffectedPlayer).WithMany(p => p.AffectedPlayerRivalryStatistics).Map(pgs => pgs.MapKey("AffectedPlayerId")).WillCascadeOnDelete(false);
            HasRequired(prs => prs.Hole).WithMany(p => p.PlayerRivalryStatistics).Map(pgs => pgs.MapKey("HoleId")).WillCascadeOnDelete(false);
            HasRequired(prs => prs.ShotType).WithMany(p => p.PlayerRivalryStatistics).Map(pgs => pgs.MapKey("ShotTypeId")).WillCascadeOnDelete(false);

            ToTable("PlayerRivalryStatistics");
        }
    }
}
