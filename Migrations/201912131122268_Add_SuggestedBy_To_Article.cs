namespace Articles_UserBased.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SuggestedBy_To_Article : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "SuggestedByUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Articles", "SuggestedByUserId");
            AddForeignKey("dbo.Articles", "SuggestedByUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "SuggestedByUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Articles", new[] { "SuggestedByUserId" });
            DropColumn("dbo.Articles", "SuggestedByUserId");
        }
    }
}
