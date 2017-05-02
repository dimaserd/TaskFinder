namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TheFiles", "TaskSolutionId", "dbo.TaskSolutions");
            DropForeignKey("dbo.TaskSolutionVersions", "FileId", "dbo.TheFiles");
            DropIndex("dbo.TaskSolutionVersions", new[] { "FileId" });
            DropIndex("dbo.TheFiles", new[] { "TaskSolutionId" });
            RenameColumn(table: "dbo.TaskSolutionVersions", name: "TaskSolutionId", newName: "FromTaskSolution_Id");
            RenameIndex(table: "dbo.TaskSolutionVersions", name: "IX_TaskSolutionId", newName: "IX_FromTaskSolution_Id");
            AddColumn("dbo.TaskSolutionVersions", "FileData", c => c.Binary());
            AddColumn("dbo.TaskSolutionVersions", "FileMymeType", c => c.String());
            DropColumn("dbo.TaskSolutionVersions", "FileId");
            DropTable("dbo.TheFiles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.TheFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FileData = c.Binary(),
                        FileMymeType = c.String(),
                        TaskSolutionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TaskSolutionVersions", "FileId", c => c.Int(nullable: false));
            DropColumn("dbo.TaskSolutionVersions", "FileMymeType");
            DropColumn("dbo.TaskSolutionVersions", "FileData");
            RenameIndex(table: "dbo.TaskSolutionVersions", name: "IX_FromTaskSolution_Id", newName: "IX_TaskSolutionId");
            RenameColumn(table: "dbo.TaskSolutionVersions", name: "FromTaskSolution_Id", newName: "TaskSolutionId");
            CreateIndex("dbo.TheFiles", "TaskSolutionId");
            CreateIndex("dbo.TaskSolutionVersions", "FileId");
            AddForeignKey("dbo.TaskSolutionVersions", "FileId", "dbo.TheFiles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TheFiles", "TaskSolutionId", "dbo.TaskSolutions", "Id");
        }
    }
}
