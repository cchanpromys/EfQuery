namespace EfQuery.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SystemStatus = c.Int(nullable: false),
                        Warehouse_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.Warehouse_Id)
                .Index(t => t.Warehouse_Id);
            
            CreateTable(
                "dbo.QuoteDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quality = c.Int(nullable: false),
                        ParentId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        SystemStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Quotes", t => t.ParentId)
                .Index(t => t.ParentId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Quotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SystemStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Warehouses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        IsInternal = c.Boolean(nullable: false),
                        IsDefaultInternal = c.Boolean(nullable: false),
                        IsServicing = c.Boolean(nullable: false),
                        IsDefaultServicing = c.Boolean(nullable: false),
                        IsReceiving = c.Boolean(nullable: false),
                        IsDefaultReceiving = c.Boolean(nullable: false),
                        IsShipping = c.Boolean(nullable: false),
                        IsDefaultShipping = c.Boolean(nullable: false),
                        IsStaging = c.Boolean(nullable: false),
                        IsDefaultStaging = c.Boolean(nullable: false),
                        IsAdjustable = c.Boolean(nullable: false),
                        SystemStatus = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SalesTerritories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SystemStatus = c.Int(nullable: false),
                        Warehouse_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Warehouses", t => t.Warehouse_Id)
                .Index(t => t.Warehouse_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Warehouse_Id", "dbo.Warehouses");
            DropForeignKey("dbo.SalesTerritories", "Warehouse_Id", "dbo.Warehouses");
            DropForeignKey("dbo.QuoteDetails", "ParentId", "dbo.Quotes");
            DropForeignKey("dbo.QuoteDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.SalesTerritories", new[] { "Warehouse_Id" });
            DropIndex("dbo.QuoteDetails", new[] { "ProductId" });
            DropIndex("dbo.QuoteDetails", new[] { "ParentId" });
            DropIndex("dbo.Products", new[] { "Warehouse_Id" });
            DropTable("dbo.SalesTerritories");
            DropTable("dbo.Warehouses");
            DropTable("dbo.Quotes");
            DropTable("dbo.QuoteDetails");
            DropTable("dbo.Products");
        }
    }
}
