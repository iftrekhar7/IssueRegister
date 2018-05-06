namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Mobile", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Mobile", c => c.String(nullable: false));
        }
    }
}
