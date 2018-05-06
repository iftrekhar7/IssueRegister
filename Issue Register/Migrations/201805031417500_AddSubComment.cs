namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubComment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        CommentDate = c.DateTime(),
                        CommentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comments", t => t.CommentId, cascadeDelete: true)
                .Index(t => t.CommentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubComments", "CommentId", "dbo.Comments");
            DropIndex("dbo.SubComments", new[] { "CommentId" });
            DropTable("dbo.SubComments");
        }
    }
}
