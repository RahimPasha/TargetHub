namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscribedServers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "Address", c => c.String());
            AddColumn("dbo.Servers", "Target_ID", c => c.Int());
            AddColumn("dbo.Targets", "ChatFilePath", c => c.String());
            CreateIndex("dbo.Servers", "Target_ID");
            AddForeignKey("dbo.Servers", "Target_ID", "dbo.Targets", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Servers", "Target_ID", "dbo.Targets");
            DropIndex("dbo.Servers", new[] { "Target_ID" });
            DropColumn("dbo.Targets", "ChatFilePath");
            DropColumn("dbo.Servers", "Target_ID");
            DropColumn("dbo.Servers", "Address");
        }
    }
}
