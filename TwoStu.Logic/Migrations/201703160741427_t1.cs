namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskSolutionVersions", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskSolutionVersions", "IsActive");
        }
    }
}
