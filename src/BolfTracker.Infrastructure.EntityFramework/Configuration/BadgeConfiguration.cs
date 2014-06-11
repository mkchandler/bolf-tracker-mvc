using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class BadgeConfiguration : EntityTypeConfiguration<Badge>
    {
        public BadgeConfiguration()
        {
            HasKey(g => g.Id);

            Property(g => g.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            ToTable("Badge");
        }
    }
}
