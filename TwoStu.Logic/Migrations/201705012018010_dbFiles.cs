namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dbFiles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TheFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileData = c.Binary(),
                        FileMymeType = c.String(),
                        TaskSolutionId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskSolutions", t => t.TaskSolutionId)
                .Index(t => t.TaskSolutionId);
            
            AddColumn("dbo.TaskSolutionVersions", "FileId", c => c.Int(nullable: false));
            CreateIndex("dbo.TaskSolutionVersions", "FileId");
            AddForeignKey("dbo.TaskSolutionVersions", "FileId", "dbo.TheFiles", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSolutionVersions", "FileId", "dbo.TheFiles");
            DropForeignKey("dbo.TheFiles", "TaskSolutionId", "dbo.TaskSolutions");
            DropIndex("dbo.TheFiles", new[] { "TaskSolutionId" });
            DropIndex("dbo.TaskSolutionVersions", new[] { "FileId" });
            DropColumn("dbo.TaskSolutionVersions", "FileId");
            DropTable("dbo.TheFiles");
        }
    }
}
