namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Subjects", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Subjects");
        }
    }
}
