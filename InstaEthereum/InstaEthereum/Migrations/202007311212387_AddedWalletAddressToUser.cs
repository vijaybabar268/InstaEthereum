namespace InstaEthereum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWalletAddressToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "WalletAddress", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "WalletAddress");
        }
    }
}
