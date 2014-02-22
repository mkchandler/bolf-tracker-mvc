namespace BolfTracker.Infrastructure.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlayerBadges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Badge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PlayerBadges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BadgeId = c.Int(nullable: false),
                        GameId = c.Int(),
                        PlayerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Badge", t => t.BadgeId, cascadeDelete: true)
                .ForeignKey("dbo.Game", t => t.GameId)
                .ForeignKey("dbo.Player", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.BadgeId)
                .Index(t => t.GameId)
                .Index(t => t.PlayerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerBadges", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.PlayerBadges", "GameId", "dbo.Game");
            DropForeignKey("dbo.PlayerBadges", "BadgeId", "dbo.Badge");
            DropIndex("dbo.PlayerBadges", new[] { "PlayerId" });
            DropIndex("dbo.PlayerBadges", new[] { "GameId" });
            DropIndex("dbo.PlayerBadges", new[] { "BadgeId" });
            DropTable("dbo.PlayerBadges");
            DropTable("dbo.Badge");
        }
    }
}
