using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework.Configuration
{
    public class RankingConfiguration : EntityTypeConfiguration<Ranking>
    {
        public RankingConfiguration()
        {
            HasKey(r => r.Id);

            Property(r => r.Id).HasDatabaseGenerationOption(DatabaseGenerationOption.Identity);

            HasRequired(r => r.Player).WithMany(p => p.Rankings);

            Ignore(r => r.TotalGames);
            Ignore(r => r.LastTenTotalGames);

            ToTable("Ranking");
        }
    }
}