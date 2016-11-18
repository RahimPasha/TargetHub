namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Start : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContentRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ContentId = c.Int(nullable: false),
                        ContentRequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Contents", t => t.ContentId, cascadeDelete: true)
                .ForeignKey("dbo.ContentRequestTypes", t => t.ContentRequestTypeId, cascadeDelete: true)
                .Index(t => t.ContentId)
                .Index(t => t.ContentRequestTypeId);
            
            CreateTable(
                "dbo.Contents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ContentRequestTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServerRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerId = c.Int(nullable: false),
                        ServerRequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servers", t => t.ServerId, cascadeDelete: true)
                .ForeignKey("dbo.ServerRequestTypes", t => t.ServerRequestTypeId, cascadeDelete: true)
                .Index(t => t.ServerId)
                .Index(t => t.ServerRequestTypeId);
            
            CreateTable(
                "dbo.Servers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Identifier = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServerRequestTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TargetRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TargetId = c.Int(nullable: false),
                        TargetRequestTypeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Targets", t => t.TargetId, cascadeDelete: true)
                .ForeignKey("dbo.TargetRequestTypes", t => t.TargetRequestTypeId, cascadeDelete: true)
                .Index(t => t.TargetId)
                .Index(t => t.TargetRequestTypeId);
            
            CreateTable(
                "dbo.Targets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        XmlFilePath = c.String(),
                        DatFilePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TargetRequestTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            //DropTable("dbo.Tests");
        }
        
        public override void Down()
        {
            /*CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);*/
            
            DropForeignKey("dbo.TargetRequests", "TargetRequestTypeId", "dbo.TargetRequestTypes");
            DropForeignKey("dbo.TargetRequests", "TargetId", "dbo.Targets");
            DropForeignKey("dbo.ServerRequests", "ServerRequestTypeId", "dbo.ServerRequestTypes");
            DropForeignKey("dbo.ServerRequests", "ServerId", "dbo.Servers");
            DropForeignKey("dbo.ContentRequests", "ContentRequestTypeId", "dbo.ContentRequestTypes");
            DropForeignKey("dbo.ContentRequests", "ContentId", "dbo.Contents");
            DropIndex("dbo.TargetRequests", new[] { "TargetRequestTypeId" });
            DropIndex("dbo.TargetRequests", new[] { "TargetId" });
            DropIndex("dbo.ServerRequests", new[] { "ServerRequestTypeId" });
            DropIndex("dbo.ServerRequests", new[] { "ServerId" });
            DropIndex("dbo.ContentRequests", new[] { "ContentRequestTypeId" });
            DropIndex("dbo.ContentRequests", new[] { "ContentId" });
            DropTable("dbo.TargetRequestTypes");
            DropTable("dbo.Targets");
            DropTable("dbo.TargetRequests");
            DropTable("dbo.ServerRequestTypes");
            DropTable("dbo.Servers");
            DropTable("dbo.ServerRequests");
            DropTable("dbo.ContentRequestTypes");
            DropTable("dbo.Contents");
            DropTable("dbo.ContentRequests");
        }
    }
}
