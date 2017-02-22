namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskSolutions", "CreationDate", c => c.DateTime(nullable: false, defaultValue: null));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskSolutions", "CreationDate");
        }
    }
}
