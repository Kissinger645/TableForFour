namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userchanges : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MatchedCouples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Suggestions = c.String(),
                        FirstCouple = c.String(maxLength: 128),
                        SecondCouple = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.FirstCouple)
                .ForeignKey("dbo.AspNetUsers", t => t.SecondCouple)
                .Index(t => t.FirstCouple)
                .Index(t => t.SecondCouple);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MatchedCouples", "SecondCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.MatchedCouples", "FirstCouple", "dbo.AspNetUsers");
            DropIndex("dbo.MatchedCouples", new[] { "SecondCouple" });
            DropIndex("dbo.MatchedCouples", new[] { "FirstCouple" });
            DropTable("dbo.MatchedCouples");
        }
    }
}
