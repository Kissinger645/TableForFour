namespace Dinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userfk : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Couples", name: "UserName_Id", newName: "CurrentUser");
            RenameIndex(table: "dbo.Couples", name: "IX_UserName_Id", newName: "IX_CurrentUser");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Couples", name: "IX_CurrentUser", newName: "IX_UserName_Id");
            RenameColumn(table: "dbo.Couples", name: "CurrentUser", newName: "UserName_Id");
        }
    }
}
