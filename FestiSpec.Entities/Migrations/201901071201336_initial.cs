namespace FestiSpec.Entities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AnswerText = c.String(),
                        Inspection_Id = c.Int(nullable: false),
                        Question_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inspections", t => t.Inspection_Id, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .Index(t => t.Inspection_Id)
                .Index(t => t.Question_Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(nullable: false, maxLength: 300),
                        Description = c.String(),
                        Index = c.Int(nullable: false),
                        QuestionOptions = c.String(maxLength: 300),
                        Type = c.String(maxLength: 45),
                        QuestionList_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionLists", t => t.QuestionList_Id)
                .ForeignKey("dbo.QuestionTypes", t => t.Type)
                .Index(t => t.Type)
                .Index(t => t.QuestionList_Id);
            
            CreateTable(
                "dbo.QuestionLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        VersionNumber = c.Double(nullable: false),
                        Festival_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Festivals", t => t.Festival_Id, cascadeDelete: true)
                .Index(t => t.Festival_Id);
            
            CreateTable(
                "dbo.Festivals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        HouseNumber = c.String(),
                        LocationComment = c.String(),
                        IsArchived = c.Boolean(nullable: false),
                        CustomerCompany_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerCompanies", t => t.CustomerCompany_Id, cascadeDelete: true)
                .Index(t => t.CustomerCompany_Id);
            
            CreateTable(
                "dbo.CustomerCompanies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NameCompany = c.String(nullable: false, maxLength: 45),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        HouseNumber = c.String(nullable: false, maxLength: 6),
                        ZipCode = c.String(nullable: false, maxLength: 6),
                        IsArchived = c.Boolean(nullable: false),
                        ContactPerson_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContactPersons", t => t.ContactPerson_Id, cascadeDelete: true)
                .Index(t => t.ContactPerson_Id);
            
            CreateTable(
                "dbo.ContactPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        TelephoneNumber = c.String(nullable: false, maxLength: 45),
                        Email = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inspections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        FestivalId = c.Int(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Present = c.Boolean(nullable: false),
                        ReasonsAbsent = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Festivals", t => t.FestivalId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.FestivalId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 45),
                        TelephoneNumber = c.String(nullable: false, maxLength: 45),
                        Email = c.String(nullable: false, maxLength: 45),
                        City = c.String(nullable: false),
                        Street = c.String(nullable: false),
                        HouseNumber = c.String(nullable: false, maxLength: 6),
                        ZipCode = c.String(nullable: false, maxLength: 6),
                        IsArchived = c.Boolean(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.Role_Id);
            
            CreateTable(
                "dbo.EmployeeAbsences",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.EmployeeId, t.Date })
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.EmployeeAccounts",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false),
                        Username = c.String(nullable: false, maxLength: 45),
                        Password = c.String(nullable: false, maxLength: 45),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PermissionNodes",
                c => new
                    {
                        Permission = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Permission);
            
            CreateTable(
                "dbo.QuestionTypes",
                c => new
                    {
                        Type = c.String(nullable: false, maxLength: 45),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Type);
            
            CreateTable(
                "dbo.AttachmentAnswers",
                c => new
                    {
                        Attachment_Id = c.Int(nullable: false),
                        Answer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Attachment_Id, t.Answer_Id })
                .ForeignKey("dbo.Attachments", t => t.Attachment_Id, cascadeDelete: true)
                .ForeignKey("dbo.Answers", t => t.Answer_Id, cascadeDelete: true)
                .Index(t => t.Attachment_Id)
                .Index(t => t.Answer_Id);
            
            CreateTable(
                "dbo.QuestionAttachments",
                c => new
                    {
                        Question_Id = c.Int(nullable: false),
                        Attachment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Question_Id, t.Attachment_Id })
                .ForeignKey("dbo.Questions", t => t.Question_Id, cascadeDelete: true)
                .ForeignKey("dbo.Attachments", t => t.Attachment_Id, cascadeDelete: true)
                .Index(t => t.Question_Id)
                .Index(t => t.Attachment_Id);
            
            CreateTable(
                "dbo.PermissionNodeRoles",
                c => new
                    {
                        PermissionNode_Permission = c.String(nullable: false, maxLength: 128),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PermissionNode_Permission, t.Role_Id })
                .ForeignKey("dbo.PermissionNodes", t => t.PermissionNode_Permission, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.PermissionNode_Permission)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Answers", "Inspection_Id", "dbo.Inspections");
            DropForeignKey("dbo.Questions", "Type", "dbo.QuestionTypes");
            DropForeignKey("dbo.Questions", "QuestionList_Id", "dbo.QuestionLists");
            DropForeignKey("dbo.QuestionLists", "Festival_Id", "dbo.Festivals");
            DropForeignKey("dbo.Inspections", "FestivalId", "dbo.Festivals");
            DropForeignKey("dbo.Inspections", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionNodeRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.PermissionNodeRoles", "PermissionNode_Permission", "dbo.PermissionNodes");
            DropForeignKey("dbo.EmployeeAccounts", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeAbsences", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Festivals", "CustomerCompany_Id", "dbo.CustomerCompanies");
            DropForeignKey("dbo.CustomerCompanies", "ContactPerson_Id", "dbo.ContactPersons");
            DropForeignKey("dbo.QuestionAttachments", "Attachment_Id", "dbo.Attachments");
            DropForeignKey("dbo.QuestionAttachments", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.AttachmentAnswers", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.AttachmentAnswers", "Attachment_Id", "dbo.Attachments");
            DropIndex("dbo.PermissionNodeRoles", new[] { "Role_Id" });
            DropIndex("dbo.PermissionNodeRoles", new[] { "PermissionNode_Permission" });
            DropIndex("dbo.QuestionAttachments", new[] { "Attachment_Id" });
            DropIndex("dbo.QuestionAttachments", new[] { "Question_Id" });
            DropIndex("dbo.AttachmentAnswers", new[] { "Answer_Id" });
            DropIndex("dbo.AttachmentAnswers", new[] { "Attachment_Id" });
            DropIndex("dbo.EmployeeAccounts", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeAbsences", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "Role_Id" });
            DropIndex("dbo.Inspections", new[] { "FestivalId" });
            DropIndex("dbo.Inspections", new[] { "EmployeeId" });
            DropIndex("dbo.CustomerCompanies", new[] { "ContactPerson_Id" });
            DropIndex("dbo.Festivals", new[] { "CustomerCompany_Id" });
            DropIndex("dbo.QuestionLists", new[] { "Festival_Id" });
            DropIndex("dbo.Questions", new[] { "QuestionList_Id" });
            DropIndex("dbo.Questions", new[] { "Type" });
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "Inspection_Id" });
            DropTable("dbo.PermissionNodeRoles");
            DropTable("dbo.QuestionAttachments");
            DropTable("dbo.AttachmentAnswers");
            DropTable("dbo.QuestionTypes");
            DropTable("dbo.PermissionNodes");
            DropTable("dbo.Roles");
            DropTable("dbo.EmployeeAccounts");
            DropTable("dbo.EmployeeAbsences");
            DropTable("dbo.Employees");
            DropTable("dbo.Inspections");
            DropTable("dbo.ContactPersons");
            DropTable("dbo.CustomerCompanies");
            DropTable("dbo.Festivals");
            DropTable("dbo.QuestionLists");
            DropTable("dbo.Questions");
            DropTable("dbo.Attachments");
            DropTable("dbo.Answers");
        }
    }
}
