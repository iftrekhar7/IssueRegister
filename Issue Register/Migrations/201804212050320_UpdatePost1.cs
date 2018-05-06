namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatePost1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Posts", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Posts", "Status", c => c.Int(nullable: false));
        }
    }
}
