namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTagstoTarget : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Targets", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Targets", "XmlFilePath", c => c.String(nullable: false));
            AlterColumn("dbo.Targets", "DatFilePath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Targets", "DatFilePath", c => c.String());
            AlterColumn("dbo.Targets", "XmlFilePath", c => c.String());
            AlterColumn("dbo.Targets", "Name", c => c.String());
        }
    }
}
