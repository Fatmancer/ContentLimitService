namespace ContentLimitService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Items", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Items", new[] { "CategoryId" });
            RenameColumn(table: "dbo.Items", name: "CategoryId", newName: "Category_Id");
            AddColumn("dbo.Items", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Items", "Category_Id", c => c.Int());
            CreateIndex("dbo.Items", "Category_Id");
            AddForeignKey("dbo.Items", "Category_Id", "dbo.Categories", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Items", new[] { "Category_Id" });
            AlterColumn("dbo.Items", "Category_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Items", "Category");
            RenameColumn(table: "dbo.Items", name: "Category_Id", newName: "CategoryId");
            CreateIndex("dbo.Items", "CategoryId");
            AddForeignKey("dbo.Items", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
