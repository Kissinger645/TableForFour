namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Couples", "ProfilePic", c => c.Int(nullable: false));
            CreateIndex("dbo.Couples", "ProfilePic");
            AddForeignKey("dbo.Couples", "ProfilePic", "dbo.ImageUploads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Couples", "ProfilePic", "dbo.ImageUploads");
            DropIndex("dbo.Couples", new[] { "ProfilePic" });
            DropColumn("dbo.Couples", "ProfilePic");
        }
    }
}
