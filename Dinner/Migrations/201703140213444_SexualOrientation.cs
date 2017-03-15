namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SexualOrientation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Couples", "Orientation", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Couples", "Orientation");
        }
    }
}
