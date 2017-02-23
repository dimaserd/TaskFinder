namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubjectDivisionChilds",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        SubjectDivisionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubjectDivisions", t => t.SubjectDivisionId, cascadeDelete: true)
                .Index(t => t.SubjectDivisionId);
            
            CreateTable(
                "dbo.SubjectDivisions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SubjectSectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubjectSections", t => t.SubjectSectionId, cascadeDelete: true)
                .Index(t => t.SubjectSectionId);
            
            CreateTable(
                "dbo.TaskSolutionSubjectDivisionChilds",
                c => new
                    {
                        TaskSolution_Id = c.String(nullable: false, maxLength: 128),
                        SubjectDivisionChild_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskSolution_Id, t.SubjectDivisionChild_Id })
                .ForeignKey("dbo.TaskSolutions", t => t.TaskSolution_Id, cascadeDelete: false)
                .ForeignKey("dbo.SubjectDivisionChilds", t => t.SubjectDivisionChild_Id, cascadeDelete: false)
                .Index(t => t.TaskSolution_Id)
                .Index(t => t.SubjectDivisionChild_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSolutionSubjectDivisionChilds", "SubjectDivisionChild_Id", "dbo.SubjectDivisionChilds");
            DropForeignKey("dbo.TaskSolutionSubjectDivisionChilds", "TaskSolution_Id", "dbo.TaskSolutions");
            DropForeignKey("dbo.SubjectDivisionChilds", "SubjectDivisionId", "dbo.SubjectDivisions");
            DropForeignKey("dbo.SubjectDivisions", "SubjectSectionId", "dbo.SubjectSections");
            DropIndex("dbo.TaskSolutionSubjectDivisionChilds", new[] { "SubjectDivisionChild_Id" });
            DropIndex("dbo.TaskSolutionSubjectDivisionChilds", new[] { "TaskSolution_Id" });
            DropIndex("dbo.SubjectDivisions", new[] { "SubjectSectionId" });
            DropIndex("dbo.SubjectDivisionChilds", new[] { "SubjectDivisionId" });
            DropTable("dbo.TaskSolutionSubjectDivisionChilds");
            DropTable("dbo.SubjectDivisions");
            DropTable("dbo.SubjectDivisionChilds");
        }
    }
}
