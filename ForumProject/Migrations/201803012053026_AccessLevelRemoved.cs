namespace ForumProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessLevelRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "AccessLevelId", "dbo.AccessLevel");
            DropIndex("dbo.Users", new[] { "AccessLevelId" });
            DropColumn("dbo.Users", "Login");
            DropColumn("dbo.Users", "Password");
            DropColumn("dbo.Users", "AccessLevelId");
            DropTable("dbo.AccessLevel");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AccessLevel",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false, maxLength: 32),
                        CanAddRecord = c.Boolean(nullable: false),
                        CanLike = c.Boolean(nullable: false),
                        CanDeleteRecord = c.Boolean(nullable: false),
                        CanDeleteUser = c.Boolean(nullable: false),
                        CanBlockUser = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "AccessLevelId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Password", c => c.String(nullable: false, maxLength: 64, fixedLength: true));
            AddColumn("dbo.Users", "Login", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Users", "AccessLevelId");
            AddForeignKey("dbo.Users", "AccessLevelId", "dbo.AccessLevel", "Id");
        }
    }
}
