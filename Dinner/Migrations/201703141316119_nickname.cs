namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nickname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Couples", "NickName", c => c.String());
            DropColumn("dbo.Couples", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Couples", "Name", c => c.String());
            DropColumn("dbo.Couples", "NickName");
        }
    }
}
