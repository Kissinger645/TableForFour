namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class couplechoices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Couples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ZipCode = c.Int(nullable: false),
                        Phone = c.String(),
                        Age = c.String(),
                        SexualPref = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ImageUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Caption = c.String(),
                        File = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImageUploads");
            DropTable("dbo.Couples");
        }
    }
}
