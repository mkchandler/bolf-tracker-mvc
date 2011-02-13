using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class ShotTypeConfiguration : EntityTypeConfiguration<ShotType>
    {
        public ShotTypeConfiguration()
        {
            HasKey(st => st.Id);

            Property(st => st.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);
            Property(st => st.Name).IsRequired().IsVariableLength().HasMaxLength(50);
            Property(st => st.Description).IsRequired().IsVariableLength().HasMaxLength(100);

            ToTable("ShotType");
        }
    }
}