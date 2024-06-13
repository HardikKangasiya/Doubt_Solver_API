namespace StackflowAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datetimechangedtodatetime2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Answers", "PostedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Questions", "PostedAt", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "PostedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Answers", "PostedAt", c => c.DateTime(nullable: false));
        }
    }
}
