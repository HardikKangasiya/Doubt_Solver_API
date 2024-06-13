namespace StackflowAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostedByIdchangedtoForeignkeyusingcodefirstapproach : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "PostedById", c => c.String(maxLength: 128));
            CreateIndex("dbo.Questions", "PostedById");
            AddForeignKey("dbo.Questions", "PostedById", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Questions", "PostedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "PostedBy", c => c.String());
            DropForeignKey("dbo.Questions", "PostedById", "dbo.AspNetUsers");
            DropIndex("dbo.Questions", new[] { "PostedById" });
            DropColumn("dbo.Questions", "PostedById");
        }
    }
}
