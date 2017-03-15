namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CoupleFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Couples", "PricePref", c => c.String());
            AddColumn("dbo.Couples", "CoupleName_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Couples", "CoupleName_Id");
            AddForeignKey("dbo.Couples", "CoupleName_Id", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Couples", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Couples", "Name", c => c.String());
            DropForeignKey("dbo.Couples", "CoupleName_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Couples", new[] { "CoupleName_Id" });
            DropColumn("dbo.Couples", "CoupleName_Id");
            DropColumn("dbo.Couples", "PricePref");
        }
    }
}
