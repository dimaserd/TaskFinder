namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TaskSolutions", "WorkTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.TaskSolutions", "WorkTypeId");
            AddForeignKey("dbo.TaskSolutions", "WorkTypeId", "dbo.WorkTypes", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskSolutions", "WorkTypeId", "dbo.WorkTypes");
            DropIndex("dbo.TaskSolutions", new[] { "WorkTypeId" });
            DropColumn("dbo.TaskSolutions", "WorkTypeId");
            DropTable("dbo.WorkTypes");
        }
    }
}
