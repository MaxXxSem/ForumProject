namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserDataUsersRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserData_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.ApplicationUsers", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "UserData_Id");
            CreateIndex("dbo.ApplicationUsers", "UserId");
            AddForeignKey("dbo.ApplicationUsers", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.Users", "UserData_Id", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserData_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUsers", "UserId", "dbo.Users");
            DropIndex("dbo.ApplicationUsers", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "UserData_Id" });
            DropColumn("dbo.ApplicationUsers", "UserId");
            DropColumn("dbo.Users", "UserData_Id");
        }
    }
}
