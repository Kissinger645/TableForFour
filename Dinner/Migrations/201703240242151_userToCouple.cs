namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userToCouple : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Likes", "ThisCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "OtherCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.MatchedCouples", "FirstCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.MatchedCouples", "SecondCouple", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "ThisCouple" });
            DropIndex("dbo.Likes", new[] { "OtherCouple" });
            DropIndex("dbo.MatchedCouples", new[] { "FirstCouple" });
            DropIndex("dbo.MatchedCouples", new[] { "SecondCouple" });
            AlterColumn("dbo.Likes", "ThisCouple", c => c.Int(nullable: false));
            AlterColumn("dbo.Likes", "OtherCouple", c => c.Int(nullable: false));
            AlterColumn("dbo.MatchedCouples", "FirstCouple", c => c.Int(nullable: false));
            AlterColumn("dbo.MatchedCouples", "SecondCouple", c => c.Int(nullable: false));
            CreateIndex("dbo.Likes", "ThisCouple");
            CreateIndex("dbo.Likes", "OtherCouple");
            CreateIndex("dbo.MatchedCouples", "FirstCouple");
            CreateIndex("dbo.MatchedCouples", "SecondCouple");
            AddForeignKey("dbo.Likes", "ThisCouple", "dbo.Couples", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Likes", "OtherCouple", "dbo.Couples", "Id", cascadeDelete: false);
            AddForeignKey("dbo.MatchedCouples", "FirstCouple", "dbo.Couples", "Id", cascadeDelete: false);
            AddForeignKey("dbo.MatchedCouples", "SecondCouple", "dbo.Couples", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchedCouples", "SecondCouple", "dbo.Couples");
            DropForeignKey("dbo.MatchedCouples", "FirstCouple", "dbo.Couples");
            DropForeignKey("dbo.Likes", "OtherCouple", "dbo.Couples");
            DropForeignKey("dbo.Likes", "ThisCouple", "dbo.Couples");
            DropIndex("dbo.MatchedCouples", new[] { "SecondCouple" });
            DropIndex("dbo.MatchedCouples", new[] { "FirstCouple" });
            DropIndex("dbo.Likes", new[] { "OtherCouple" });
            DropIndex("dbo.Likes", new[] { "ThisCouple" });
            AlterColumn("dbo.MatchedCouples", "SecondCouple", c => c.String(maxLength: 128));
            AlterColumn("dbo.MatchedCouples", "FirstCouple", c => c.String(maxLength: 128));
            AlterColumn("dbo.Likes", "OtherCouple", c => c.String(maxLength: 128));
            AlterColumn("dbo.Likes", "ThisCouple", c => c.String(maxLength: 128));
            CreateIndex("dbo.MatchedCouples", "SecondCouple");
            CreateIndex("dbo.MatchedCouples", "FirstCouple");
            CreateIndex("dbo.Likes", "OtherCouple");
            CreateIndex("dbo.Likes", "ThisCouple");
            AddForeignKey("dbo.MatchedCouples", "SecondCouple", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.MatchedCouples", "FirstCouple", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Likes", "OtherCouple", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Likes", "ThisCouple", "dbo.AspNetUsers", "Id");
        }
    }
}
