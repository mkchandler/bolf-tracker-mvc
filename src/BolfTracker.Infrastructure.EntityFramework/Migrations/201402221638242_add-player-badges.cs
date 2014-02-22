namespace BolfTracker.Infrastructure.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addplayerbadges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PlayerBadges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        BadgeId = c.Int(nullable: false),
                        GameId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Badges", t => t.BadgeId, cascadeDelete: true)
                .ForeignKey("dbo.Game", t => t.GameId)
                .ForeignKey("dbo.Player", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.BadgeId)
                .Index(t => t.GameId)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.Badges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PlayerBadges", "PlayerId", "dbo.Player");
            DropForeignKey("dbo.PlayerBadges", "GameId", "dbo.Game");
            DropForeignKey("dbo.PlayerBadges", "BadgeId", "dbo.Badges");
            DropIndex("dbo.PlayerBadges", new[] { "PlayerId" });
            DropIndex("dbo.PlayerBadges", new[] { "GameId" });
            DropIndex("dbo.PlayerBadges", new[] { "BadgeId" });
            DropTable("dbo.Badges");
            DropTable("dbo.PlayerBadges");
        }
    }
}
