namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrpInEmpVend : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IsConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Vendors", "IsConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Vendors", "IsConfirmed");
            DropColumn("dbo.Employees", "IsConfirmed");
        }
    }
}
