namespace StackflowAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedQuestionandAnswermodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        PostedAt = c.DateTime(nullable: false),
                        PostedBy_Id = c.String(maxLength: 128),
                        Question_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PostedBy_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.PostedBy_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        PostedAt = c.DateTime(nullable: false),
                        PostedBy_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.PostedBy_Id)
                .Index(t => t.PostedBy_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Questions", "PostedBy_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Answers", "PostedBy_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Questions", new[] { "PostedBy_Id" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "PostedBy_Id" });
            DropTable("dbo.Questions");
            DropTable("dbo.Answers");
        }
    }
}
