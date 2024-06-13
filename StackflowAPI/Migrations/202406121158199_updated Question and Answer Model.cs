   namespace StackflowAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedQuestionandAnswerModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Answers", "Content", c => c.String(nullable: false));
            AlterColumn("dbo.Questions", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Questions", "Content", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Content", c => c.String());
            AlterColumn("dbo.Questions", "Title", c => c.String());
            AlterColumn("dbo.Answers", "Content", c => c.String());
        }
    }
}
