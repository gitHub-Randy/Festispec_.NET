namespace FestiSpec.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inspections", "FestivalId", "dbo.Festivals");
            RenameColumn(table: "dbo.Inspections", name: "FestivalId", newName: "Festival_Id");
            AddColumn("dbo.Inspections", "QuestionList_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Inspections", "Festival_Id", c => c.Int());
            CreateIndex("dbo.Inspections", "QuestionList_Id");
            CreateIndex("dbo.Inspections", "Festival_Id");
            AddForeignKey("dbo.Inspections", "QuestionList_Id", "dbo.QuestionLists", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Inspections", "Festival_Id", "dbo.Festivals", "Id");
            DropIndex("dbo.Inspections", new[] { "FestivalId" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inspections", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Inspections", "QuestionList_Id", "dbo.QuestionLists");
            DropIndex("dbo.Inspections", new[] { "QuestionList_Id" });
            AlterColumn("dbo.Inspections", "Festival_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Inspections", "QuestionList_Id");
            RenameColumn(table: "dbo.Inspections", name: "Festival_Id", newName: "FestivalId");
            CreateIndex("dbo.Inspections", "FestivalId");
            AddForeignKey("dbo.Inspections", "FestivalId", "dbo.Festivals", "Id", cascadeDelete: true);
            DropIndex("dbo.Inspections", new[] { "Festival_Id" });
        }
    }
}
