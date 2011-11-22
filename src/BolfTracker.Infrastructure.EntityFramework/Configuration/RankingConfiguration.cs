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

            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            HasRequired(r => r.Player).WithMany(p => p.Rankings).Map(r => r.MapKey("PlayerId"));

            Ignore(r => r.TotalGames);
            Ignore(r => r.LastTenTotalGames);

            ToTable("Ranking");
        }
    }
}