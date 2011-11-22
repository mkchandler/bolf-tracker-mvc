using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            HasKey(g => g.Id);

            Property(g => g.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("Game");
        }
    }
}