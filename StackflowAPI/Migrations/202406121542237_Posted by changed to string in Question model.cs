namespace StackflowAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostedbychangedtostringinQuestionmodel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Questions", "PostedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Questions", new[] { "PostedBy_Id" });
            AddColumn("dbo.Questions", "PostedBy", c => c.String());
            DropColumn("dbo.Questions", "PostedBy_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "PostedBy_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.Questions", "PostedBy");
            CreateIndex("dbo.Questions", "PostedBy_Id");
            AddForeignKey("dbo.Questions", "PostedBy_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
