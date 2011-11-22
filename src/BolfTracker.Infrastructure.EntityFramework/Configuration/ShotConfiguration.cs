using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class ShotConfiguration : EntityTypeConfiguration<Shot>
    {
        public ShotConfiguration()
        {
            HasKey(s => s.Id);

            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(s => s.Game).WithMany(g => g.Shots).Map(s => s.MapKey("GameId"));
            HasRequired(s => s.Player).WithMany(p => p.Shots).Map(s => s.MapKey("PlayerId"));
            HasRequired(s => s.Hole).WithMany(h => h.Shots).Map(s => s.MapKey("HoleId"));
            HasRequired(s => s.ShotType).WithMany(st => st.Shots).Map(s => s.MapKey("ShotTypeId"));

            ToTable("Shot");
        }
    }
}