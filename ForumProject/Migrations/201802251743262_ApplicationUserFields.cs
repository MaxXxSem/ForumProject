namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "Name", c => c.String(nullable: false, maxLength: 64));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "Name");
        }
    }
}
