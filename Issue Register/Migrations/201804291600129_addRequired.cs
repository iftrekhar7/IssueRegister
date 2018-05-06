namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Vendors", "Mobile", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Vendors", "Mobile", c => c.String());
            AlterColumn("dbo.Employees", "Mobile", c => c.String());
        }
    }
}
