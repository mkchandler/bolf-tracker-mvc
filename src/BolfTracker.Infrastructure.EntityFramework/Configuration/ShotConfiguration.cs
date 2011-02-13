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

            Property(s => s.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(s => s.Game).WithMany(g => g.Shots);
            HasRequired(s => s.Player).WithMany(p => p.Shots);
            HasRequired(s => s.Hole).WithMany(h => h.Shots);
            HasRequired(s => s.ShotType).WithMany(st => st.Shots);

            ToTable("Shot");
        }
    }
}