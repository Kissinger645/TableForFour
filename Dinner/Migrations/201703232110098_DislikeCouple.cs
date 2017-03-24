namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DislikeCouple : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Dislikes", "ThisCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dislikes", "OtherCouple", "dbo.AspNetUsers");
            DropIndex("dbo.Dislikes", new[] { "ThisCouple" });
            DropIndex("dbo.Dislikes", new[] { "OtherCouple" });
            AlterColumn("dbo.Dislikes", "ThisCouple", c => c.Int(nullable: false));
            AlterColumn("dbo.Dislikes", "OtherCouple", c => c.Int(nullable: false));
            CreateIndex("dbo.Dislikes", "ThisCouple");
            CreateIndex("dbo.Dislikes", "OtherCouple");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dislikes", "OtherCouple", "dbo.Couples");
            DropForeignKey("dbo.Dislikes", "ThisCouple", "dbo.Couples");
            DropIndex("dbo.Dislikes", new[] { "OtherCouple" });
            DropIndex("dbo.Dislikes", new[] { "ThisCouple" });
            AlterColumn("dbo.Dislikes", "OtherCouple", c => c.String(maxLength: 128));
            AlterColumn("dbo.Dislikes", "ThisCouple", c => c.String(maxLength: 128));
            CreateIndex("dbo.Dislikes", "OtherCouple");
            CreateIndex("dbo.Dislikes", "ThisCouple");
            AddForeignKey("dbo.Dislikes", "OtherCouple", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Dislikes", "ThisCouple", "dbo.AspNetUsers", "Id");
        }
    }
}
