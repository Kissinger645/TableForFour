namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Couples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CurrentUser = c.String(maxLength: 128),
                        UserName = c.String(),
                        Bio = c.String(),
                        ProfilePic = c.Int(nullable: false),
                        ZipCode = c.Int(nullable: false),
                        Phone = c.String(),
                        Age = c.String(),
                        Orientation = c.String(),
                        FavoriteFoods = c.String(),
                        AgePreference = c.String(),
                        SexualPreference = c.String(),
                        PricePreference = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CurrentUser)
                .ForeignKey("dbo.ImageUploads", t => t.ProfilePic, cascadeDelete: true)
                .Index(t => t.CurrentUser)
                .Index(t => t.ProfilePic);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ImageUploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Caption = c.String(),
                        File = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dislikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThisCouple = c.Int(nullable: false),
                        OtherCouple = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Couples", t => t.ThisCouple, cascadeDelete: true)
                .ForeignKey("dbo.Couples", t => t.OtherCouple, cascadeDelete: true)
                .Index(t => t.ThisCouple)
                .Index(t => t.OtherCouple);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ThisCouple = c.Int(nullable: false),
                        OtherCouple = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Couples", t => t.ThisCouple, cascadeDelete: true)
                .ForeignKey("dbo.Couples", t => t.OtherCouple, cascadeDelete: true)
                .Index(t => t.ThisCouple)
                .Index(t => t.OtherCouple);
            
            CreateTable(
                "dbo.MatchedCouples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Suggestions = c.String(),
                        FirstCouple = c.Int(nullable: false),
                        SecondCouple = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Couples", t => t.FirstCouple, cascadeDelete: true)
                .ForeignKey("dbo.Couples", t => t.SecondCouple, cascadeDelete: true)
                .Index(t => t.FirstCouple)
                .Index(t => t.SecondCouple);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromCouple = c.String(),
                        ToCouple = c.String(),
                        Title = c.String(),
                        Message = c.String(),
                        Created = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.MatchedCouples", "SecondCouple", "dbo.Couples");
            DropForeignKey("dbo.MatchedCouples", "FirstCouple", "dbo.Couples");
            DropForeignKey("dbo.Likes", "OtherCouple", "dbo.Couples");
            DropForeignKey("dbo.Likes", "ThisCouple", "dbo.Couples");
            DropForeignKey("dbo.Dislikes", "OtherCouple", "dbo.Couples");
            DropForeignKey("dbo.Dislikes", "ThisCouple", "dbo.Couples");
            DropForeignKey("dbo.Couples", "ProfilePic", "dbo.ImageUploads");
            DropForeignKey("dbo.Couples", "CurrentUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MatchedCouples", new[] { "SecondCouple" });
            DropIndex("dbo.MatchedCouples", new[] { "FirstCouple" });
            DropIndex("dbo.Likes", new[] { "OtherCouple" });
            DropIndex("dbo.Likes", new[] { "ThisCouple" });
            DropIndex("dbo.Dislikes", new[] { "OtherCouple" });
            DropIndex("dbo.Dislikes", new[] { "ThisCouple" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Couples", new[] { "ProfilePic" });
            DropIndex("dbo.Couples", new[] { "CurrentUser" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Messages");
            DropTable("dbo.MatchedCouples");
            DropTable("dbo.Likes");
            DropTable("dbo.Dislikes");
            DropTable("dbo.ImageUploads");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Couples");
        }
    }
}
