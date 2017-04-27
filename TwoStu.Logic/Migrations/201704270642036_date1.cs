namespace TwoStu.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class date1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ExpirationDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ExpirationDate");
        }
    }
}
