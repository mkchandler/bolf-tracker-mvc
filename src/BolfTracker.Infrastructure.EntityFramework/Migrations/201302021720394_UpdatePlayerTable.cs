namespace BolfTracker.Infrastructure.EntityFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePlayerTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Player", "Name", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Player", "Initials", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Player", "Initials", c => c.String());
            AlterColumn("dbo.Player", "Name", c => c.String(nullable: false));
        }
    }
}
