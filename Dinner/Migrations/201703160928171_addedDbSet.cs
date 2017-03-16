namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedDbSet : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThisCouple = c.String(maxLength: 128),
                        OtherCouple = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ThisCouple)
                .ForeignKey("dbo.AspNetUsers", t => t.OtherCouple)
                .Index(t => t.ThisCouple)
                .Index(t => t.OtherCouple);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Likes", "OtherCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.Likes", "ThisCouple", "dbo.AspNetUsers");
            DropIndex("dbo.Likes", new[] { "OtherCouple" });
            DropIndex("dbo.Likes", new[] { "ThisCouple" });
            DropTable("dbo.Likes");
        }
    }
}
