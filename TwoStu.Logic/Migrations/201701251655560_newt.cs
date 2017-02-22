namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskSolutions", "SubjectId", c => c.Int(nullable: false));
            AddColumn("dbo.TaskSolutions", "SubjectSectionId", c => c.Int(nullable: false));
            CreateIndex("dbo.TaskSolutions", "SubjectId");
            CreateIndex("dbo.TaskSolutions", "SubjectSectionId");
            AddForeignKey("dbo.TaskSolutions", "SubjectId", "dbo.Subjects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TaskSolutions", "SubjectSectionId", "dbo.SubjectSections", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSolutions", "SubjectSectionId", "dbo.SubjectSections");
            DropForeignKey("dbo.TaskSolutions", "SubjectId", "dbo.Subjects");
            DropIndex("dbo.TaskSolutions", new[] { "SubjectSectionId" });
            DropIndex("dbo.TaskSolutions", new[] { "SubjectId" });
            DropColumn("dbo.TaskSolutions", "SubjectSectionId");
            DropColumn("dbo.TaskSolutions", "SubjectId");
        }
    }
}
