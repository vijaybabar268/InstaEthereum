namespace InstaEthereum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetNullableWalletAddress : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "WalletAddress", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "WalletAddress", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
