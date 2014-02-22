﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using BolfTracker.Models;

namespace BolfTracker.Infrastructure.EntityFramework
{
     public class PlayerBadgesConfiguration:EntityTypeConfiguration<PlayerBadges>
    {

         public PlayerBadgesConfiguration()
         {
             HasKey(pb => pb.Id);
             Property(pb => pb.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
             HasRequired(pb => pb.Player).WithMany(p => p.PlayerBadges).HasForeignKey(pb=>pb.PlayerId);
             HasOptional(pb => pb.Game).WithMany(p => p.PlayerBadges).HasForeignKey(pb=>pb.GameId);
             HasRequired(pb => pb.Badge).WithMany(p => p.PlayerBadges).HasForeignKey(pb=>pb.BadgeId);
         }
    }
}
