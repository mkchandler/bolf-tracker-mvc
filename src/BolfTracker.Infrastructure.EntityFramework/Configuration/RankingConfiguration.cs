using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
    public class RankingConfiguration : EntityTypeConfiguration<Ranking>
    {
        public RankingConfiguration()
        {
            HasKey(r => r.Id);

            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.WinningPercentage).HasPrecision(18, 3);
            Property(r => r.LastTenWinningPercentage).HasPrecision(18, 3);
            Property(r => r.GamesBack).HasPrecision(18, 1);

            HasRequired(r => r.Player).WithMany(p => p.Rankings).Map(r => r.MapKey("PlayerId"));

            Ignore(r => r.TotalGames);
            Ignore(r => r.LastTenTotalGames);

            ToTable("Ranking");
        }
    }
}