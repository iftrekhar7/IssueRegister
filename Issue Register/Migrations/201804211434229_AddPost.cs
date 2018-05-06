namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPost : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateTime = c.DateTime(),
                        Photo = c.Binary(),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        CompanyId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.EmployeeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Posts", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Posts", new[] { "EmployeeId" });
            DropIndex("dbo.Posts", new[] { "CompanyId" });
            DropTable("dbo.Posts");
        }
    }
}
