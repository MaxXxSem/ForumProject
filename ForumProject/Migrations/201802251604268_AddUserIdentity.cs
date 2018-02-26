namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdentity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false, maxLength: 64),
                        PasswordHash = c.String(nullable: false),
                        SecurityStamp = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "UserData_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Users", "UserData_Id");
            AddForeignKey("dbo.Users", "UserData_Id", "dbo.ApplicationUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserData_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationUsers", "UserId", "dbo.Users");
            DropIndex("dbo.ApplicationUsers", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "UserData_Id" });
            DropColumn("dbo.Users", "UserData_Id");
            DropTable("dbo.ApplicationUsers");
        }
    }
}
