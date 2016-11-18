namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class servername_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Servers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Servers", "Name");
        }
    }
}
