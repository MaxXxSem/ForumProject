namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StandartIdentityRole : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ApplicationRoles", "Name", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
