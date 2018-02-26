namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Back : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ApplicationUsers", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "Name", c => c.String(nullable: false, maxLength: 64));
        }
    }
}
