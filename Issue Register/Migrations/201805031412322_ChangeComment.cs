namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Message", c => c.String());
            AddColumn("dbo.Comments", "CommentDate", c => c.DateTime());
            DropColumn("dbo.Comments", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Description", c => c.String());
            DropColumn("dbo.Comments", "CommentDate");
            DropColumn("dbo.Comments", "Message");
        }
    }
}
