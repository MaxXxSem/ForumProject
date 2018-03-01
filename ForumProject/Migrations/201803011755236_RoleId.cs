namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RoleId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", "dbo.ApplicationRoles");
            DropIndex("dbo.ApplicationRoleApplicationUsers", new[] { "ApplicationRole_Id" });
            DropPrimaryKey("dbo.ApplicationRoles");
            DropPrimaryKey("dbo.ApplicationRoleApplicationUsers");
            AlterColumn("dbo.ApplicationRoles", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.ApplicationRoles", "Id");
            AddPrimaryKey("dbo.ApplicationRoleApplicationUsers", new[] { "ApplicationRole_Id", "ApplicationUser_Id" });
            CreateIndex("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id");
            AddForeignKey("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", "dbo.ApplicationRoles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", "dbo.ApplicationRoles");
            DropIndex("dbo.ApplicationRoleApplicationUsers", new[] { "ApplicationRole_Id" });
            DropPrimaryKey("dbo.ApplicationRoleApplicationUsers");
            DropPrimaryKey("dbo.ApplicationRoles");
            AlterColumn("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", c => c.Guid(nullable: false));
            AlterColumn("dbo.ApplicationRoles", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.ApplicationRoleApplicationUsers", new[] { "ApplicationRole_Id", "ApplicationUser_Id" });
            AddPrimaryKey("dbo.ApplicationRoles", "Id");
            CreateIndex("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id");
            AddForeignKey("dbo.ApplicationRoleApplicationUsers", "ApplicationRole_Id", "dbo.ApplicationRoles", "Id", cascadeDelete: true);
        }
    }
}
