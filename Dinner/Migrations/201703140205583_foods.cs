namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class foods : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Couples", name: "CoupleName_Id", newName: "UserName_Id");
            RenameIndex(table: "dbo.Couples", name: "IX_CoupleName_Id", newName: "IX_UserName_Id");
            AddColumn("dbo.Couples", "Name", c => c.String());
            AddColumn("dbo.Couples", "Bio", c => c.String());
            AddColumn("dbo.Couples", "FavoriteFoods", c => c.Int(nullable: false));
            AddColumn("dbo.Couples", "AgePreference", c => c.String());
            AddColumn("dbo.Couples", "SexualPreference", c => c.String());
            AddColumn("dbo.Couples", "PricePreference", c => c.String());
            DropColumn("dbo.Couples", "SexualPref");
            DropColumn("dbo.Couples", "PricePref");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Couples", "PricePref", c => c.String());
            AddColumn("dbo.Couples", "SexualPref", c => c.String());
            DropColumn("dbo.Couples", "PricePreference");
            DropColumn("dbo.Couples", "SexualPreference");
            DropColumn("dbo.Couples", "AgePreference");
            DropColumn("dbo.Couples", "FavoriteFoods");
            DropColumn("dbo.Couples", "Bio");
            DropColumn("dbo.Couples", "Name");
            RenameIndex(table: "dbo.Couples", name: "IX_UserName_Id", newName: "IX_CoupleName_Id");
            RenameColumn(table: "dbo.Couples", name: "UserName_Id", newName: "CoupleName_Id");
        }
    }
}
