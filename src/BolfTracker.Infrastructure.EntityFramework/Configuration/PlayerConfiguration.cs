using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class PlayerConfiguration : EntityTypeConfiguration<Player>
    {
        public PlayerConfiguration()
        {
            HasKey(p => p.Id);

            Property(p => p.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            ToTable("Player");
        }
    }
}