namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Spat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskSolutionVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskSolutionId = c.String(maxLength: 128),
                        VersionDate = c.DateTime(nullable: false),
                        TaskDesc = c.String(),
                        TaskDescFromFile = c.String(),
                        TrimmedTaskDesc = c.String(),
                        FilePath = c.String(),
                        FileName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskSolutions", t => t.TaskSolutionId)
                .Index(t => t.TaskSolutionId);
            
            CreateTable(
                "dbo.TaskSolutionVersionSubjectDivisionChilds",
                c => new
                    {
                        TaskSolutionVersion_Id = c.Int(nullable: false),
                        SubjectDivisionChild_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.TaskSolutionVersion_Id, t.SubjectDivisionChild_Id })
                .ForeignKey("dbo.TaskSolutionVersions", t => t.TaskSolutionVersion_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubjectDivisionChilds", t => t.SubjectDivisionChild_Id, cascadeDelete: true)
                .Index(t => t.TaskSolutionVersion_Id)
                .Index(t => t.SubjectDivisionChild_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSolutionVersionSubjectDivisionChilds", "SubjectDivisionChild_Id", "dbo.SubjectDivisionChilds");
            DropForeignKey("dbo.TaskSolutionVersionSubjectDivisionChilds", "TaskSolutionVersion_Id", "dbo.TaskSolutionVersions");
            DropForeignKey("dbo.TaskSolutionVersions", "TaskSolutionId", "dbo.TaskSolutions");
            DropIndex("dbo.TaskSolutionVersionSubjectDivisionChilds", new[] { "SubjectDivisionChild_Id" });
            DropIndex("dbo.TaskSolutionVersionSubjectDivisionChilds", new[] { "TaskSolutionVersion_Id" });
            DropIndex("dbo.TaskSolutionVersions", new[] { "TaskSolutionId" });
            DropTable("dbo.TaskSolutionVersionSubjectDivisionChilds");
            DropTable("dbo.TaskSolutionVersions");
        }
    }
}
