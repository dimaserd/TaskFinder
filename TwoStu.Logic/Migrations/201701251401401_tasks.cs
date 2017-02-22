namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tasks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskSolutions", "TaskDescFromFile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskSolutions", "TaskDescFromFile");
        }
    }
}
