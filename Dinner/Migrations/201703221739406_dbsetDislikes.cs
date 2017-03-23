namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbsetDislikes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dislikes",
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
            DropForeignKey("dbo.Dislikes", "OtherCouple", "dbo.AspNetUsers");
            DropForeignKey("dbo.Dislikes", "ThisCouple", "dbo.AspNetUsers");
            DropIndex("dbo.Dislikes", new[] { "OtherCouple" });
            DropIndex("dbo.Dislikes", new[] { "ThisCouple" });
            DropTable("dbo.Dislikes");
        }
    }
}
