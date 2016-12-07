namespace TargetHubApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTagstoTarget21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Targets", "XmlFilePath", c => c.String());
            AlterColumn("dbo.Targets", "DatFilePath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Targets", "DatFilePath", c => c.String(nullable: false));
            AlterColumn("dbo.Targets", "XmlFilePath", c => c.String(nullable: false));
        }
    }
}
