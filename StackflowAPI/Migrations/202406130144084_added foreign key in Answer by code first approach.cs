namespace StackflowAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedforeignkeyinAnswerbycodefirstapproach : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            RenameColumn(table: "dbo.Answers", name: "PostedBy_Id", newName: "PostedById");
            RenameColumn(table: "dbo.Answers", name: "Question_Id", newName: "QuestionId");
            RenameIndex(table: "dbo.Answers", name: "IX_PostedBy_Id", newName: "IX_PostedById");
            AlterColumn("dbo.Answers", "QuestionId", c => c.Int(nullable: false));
            CreateIndex("dbo.Answers", "QuestionId");
            AddForeignKey("dbo.Answers", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Answers", new[] { "QuestionId" });
            AlterColumn("dbo.Answers", "QuestionId", c => c.Int());
            RenameIndex(table: "dbo.Answers", name: "IX_PostedById", newName: "IX_PostedBy_Id");
            RenameColumn(table: "dbo.Answers", name: "QuestionId", newName: "Question_Id");
            RenameColumn(table: "dbo.Answers", name: "PostedById", newName: "PostedBy_Id");
            CreateIndex("dbo.Answers", "Question_Id");
            AddForeignKey("dbo.Answers", "Question_Id", "dbo.Questions", "Id");
        }
    }
}
