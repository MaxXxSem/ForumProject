namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UserData_Id", "dbo.ApplicationUsers");
            DropIndex("dbo.Users", new[] { "UserData_Id" });
            DropIndex("dbo.ApplicationUsers", new[] { "UserId" });
            DropColumn("dbo.Users", "UserData_Id");
            DropColumn("dbo.ApplicationUsers", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ApplicationUsers", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "UserData_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.ApplicationUsers", "UserId");
            CreateIndex("dbo.Users", "UserData_Id");
            AddForeignKey("dbo.Users", "UserData_Id", "dbo.ApplicationUsers", "Id");
            AddForeignKey("dbo.ApplicationUsers", "UserId", "dbo.Users", "Id");
        }
    }
}
