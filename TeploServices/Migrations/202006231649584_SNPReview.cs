namespace TeploServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SNPReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "SNP", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "SNP");
        }
    }
}
