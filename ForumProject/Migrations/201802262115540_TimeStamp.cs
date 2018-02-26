namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TimeStamp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ApplicationUsers", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ApplicationUsers", "RowVersion");
        }
    }
}
