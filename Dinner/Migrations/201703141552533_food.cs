namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class food : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Couples", "FavoriteFoods", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Couples", "FavoriteFoods", c => c.Int(nullable: false));
        }
    }
}
