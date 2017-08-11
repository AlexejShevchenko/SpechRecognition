namespace OneInc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Results",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Voice = c.Binary(),
                        OptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Options", t => t.OptionId, cascadeDelete: true)
                .Index(t => t.OptionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Results", "OptionId", "dbo.Options");
            DropForeignKey("dbo.Options", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Results", new[] { "OptionId" });
            DropIndex("dbo.Options", new[] { "QuestionId" });
            DropTable("dbo.Results");
            DropTable("dbo.Questions");
            DropTable("dbo.Options");
        }
    }
}
