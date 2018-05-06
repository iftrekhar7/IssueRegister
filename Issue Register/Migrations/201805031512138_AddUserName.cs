namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "UserName", c => c.String());
            AddColumn("dbo.SubComments", "UserName", c => c.String());
            DropColumn("dbo.Comments", "UserId");
            DropColumn("dbo.SubComments", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SubComments", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.SubComments", "UserName");
            DropColumn("dbo.Comments", "UserName");
        }
    }
}
