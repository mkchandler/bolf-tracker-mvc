using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class HoleConfiguration : EntityTypeConfiguration<Hole>
    {
        public HoleConfiguration()
        {
            HasKey(h => h.Id);

            Property(h => h.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            ToTable("Hole");
        }
    }
}