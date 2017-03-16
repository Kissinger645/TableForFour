namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Couples", "UserName", c => c.String());
            DropColumn("dbo.Couples", "NickName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Couples", "NickName", c => c.String());
            DropColumn("dbo.Couples", "UserName");
        }
    }
}
