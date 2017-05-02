namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dsrtwe : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TheFiles", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TheFiles", "Name");
        }
    }
}
