namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePost : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "Branch", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "Branch");
        }
    }
}
