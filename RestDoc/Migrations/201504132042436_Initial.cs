namespace RestDoc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiBodies",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        Example = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApiParameters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 4000),
                        DataType = c.String(nullable: false, maxLength: 100),
                        Required = c.Boolean(nullable: false),
                        DefaultValue = c.String(maxLength: 150),
                        ApiVerbId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiVerbs", t => t.ApiVerbId, cascadeDelete: true)
                .Index(t => t.ApiVerbId);
            
            CreateTable(
                "dbo.ApiVerbs",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Verb = c.String(nullable: false, maxLength: 15),
                        RequestBodyId = c.Guid(),
                        ResponseBodyId = c.Guid(nullable: false),
                        ApiPathId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiPaths", t => t.ApiPathId, cascadeDelete: true)
                .ForeignKey("dbo.ApiBodies", t => t.RequestBodyId)
                .ForeignKey("dbo.ApiBodies", t => t.ResponseBodyId, cascadeDelete: true)
                .Index(t => t.RequestBodyId)
                .Index(t => t.ResponseBodyId)
                .Index(t => t.ApiPathId);
            
            CreateTable(
                "dbo.ApiPaths",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Path = c.String(nullable: false, maxLength: 400),
                        Description = c.String(),
                        ApiPathGroupId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiPathGroups", t => t.ApiPathGroupId, cascadeDelete: true)
                .Index(t => t.ApiPathGroupId);
            
            CreateTable(
                "dbo.ApiPathGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        BasePath = c.String(nullable: false, maxLength: 400),
                        Description = c.String(maxLength: 4000),
                        RestApiId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RestApis", t => t.RestApiId, cascadeDelete: true)
                .Index(t => t.RestApiId);
            
            CreateTable(
                "dbo.RestApis",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 400),
                        Description = c.String(maxLength: 4000),
                        Creator = c.String(),
                        Version = c.String(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        LastModified = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApiStatusCodes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StatusCode = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 300),
                        ApiVerbId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApiVerbs", t => t.ApiVerbId, cascadeDelete: true)
                .Index(t => t.ApiVerbId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApiVerbs", "ResponseBodyId", "dbo.ApiBodies");
            DropForeignKey("dbo.ApiVerbs", "RequestBodyId", "dbo.ApiBodies");
            DropForeignKey("dbo.ApiParameters", "ApiVerbId", "dbo.ApiVerbs");
            DropForeignKey("dbo.ApiStatusCodes", "ApiVerbId", "dbo.ApiVerbs");
            DropForeignKey("dbo.ApiVerbs", "ApiPathId", "dbo.ApiPaths");
            DropForeignKey("dbo.ApiPathGroups", "RestApiId", "dbo.RestApis");
            DropForeignKey("dbo.ApiPaths", "ApiPathGroupId", "dbo.ApiPathGroups");
            DropIndex("dbo.ApiStatusCodes", new[] { "ApiVerbId" });
            DropIndex("dbo.ApiPathGroups", new[] { "RestApiId" });
            DropIndex("dbo.ApiPaths", new[] { "ApiPathGroupId" });
            DropIndex("dbo.ApiVerbs", new[] { "ApiPathId" });
            DropIndex("dbo.ApiVerbs", new[] { "ResponseBodyId" });
            DropIndex("dbo.ApiVerbs", new[] { "RequestBodyId" });
            DropIndex("dbo.ApiParameters", new[] { "ApiVerbId" });
            DropTable("dbo.ApiStatusCodes");
            DropTable("dbo.RestApis");
            DropTable("dbo.ApiPathGroups");
            DropTable("dbo.ApiPaths");
            DropTable("dbo.ApiVerbs");
            DropTable("dbo.ApiParameters");
            DropTable("dbo.ApiBodies");
        }
    }
}
