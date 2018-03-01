namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRoles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 64),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationRoleApplicationUsers",
                c => new
                    {
                        ApplicationRole_Id = c.String(nullable: false, maxLength: 128),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ApplicationRole_Id, t.ApplicationUser_Id })
                .ForeignKey("dbo.ApplicationRoles", t => t.ApplicationRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .Index(t => t.ApplicationRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationRoleApplicationUsers", "ApplicationUser_Id", "dbo.ApplicationUsers");
            DropForeignKey("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", "dbo.ApplicationRoles");
            DropIndex("dbo.ApplicationRoleApplicationUsers", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationRoleApplicationUsers", new[] { "ApplicationRole_Id" });
            DropTable("dbo.ApplicationRoleApplicationUsers");
            DropTable("dbo.ApplicationRoles");
        }
    }
}
