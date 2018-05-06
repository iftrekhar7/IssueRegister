namespace Issue_Register.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removePassword : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Employees", "Password");
            DropColumn("dbo.Employees", "ConfirmPassword");
            DropColumn("dbo.Vendors", "Password");
            DropColumn("dbo.Vendors", "ConfirmPassword");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Vendors", "ConfirmPassword", c => c.String());
            AddColumn("dbo.Vendors", "Password", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Employees", "ConfirmPassword", c => c.String());
            AddColumn("dbo.Employees", "Password", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
