namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSubscribedServers1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Servers", "Target_ID", "dbo.Targets");
            DropIndex("dbo.Servers", new[] { "Target_ID" });
            CreateTable(
                "dbo.Subscriptions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TargetID = c.Int(nullable: false),
                        ServerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Servers", t => t.ServerID, cascadeDelete: true)
                .ForeignKey("dbo.Targets", t => t.TargetID, cascadeDelete: true)
                .Index(t => t.TargetID)
                .Index(t => t.ServerID);
            
            DropColumn("dbo.Servers", "Target_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servers", "Target_ID", c => c.Int());
            DropForeignKey("dbo.Subscriptions", "TargetID", "dbo.Targets");
            DropForeignKey("dbo.Subscriptions", "ServerID", "dbo.Servers");
            DropIndex("dbo.Subscriptions", new[] { "ServerID" });
            DropIndex("dbo.Subscriptions", new[] { "TargetID" });
            DropTable("dbo.Subscriptions");
            CreateIndex("dbo.Servers", "Target_ID");
            AddForeignKey("dbo.Servers", "Target_ID", "dbo.Targets", "ID");
        }
    }
}
