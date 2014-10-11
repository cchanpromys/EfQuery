using System.Data.Entity.ModelConfiguration;

namespace EfQuery.Mappings
{
    public class QuoteMap: EntityTypeConfiguration<Quote>
    {
    }

    public class QuoteDetailMap : EntityTypeConfiguration<QuoteDetail>
    {
        public QuoteDetailMap()
        {
            ToTable("QuoteDetails");
            HasKey(x => x.Id);
            HasRequired(x => x.Quote)
                .WithMany(x => x.QuoteDetails)
                .HasForeignKey(x => x.ParentId);

            HasRequired(x => x.Product)
                .WithMany(x => x.QuoteDetails)
                .HasForeignKey(x => x.ProductId);
        }
    }

    public class ProductMap: EntityTypeConfiguration<Product>
    {
    }

    public class FocusedWarehouseMap : EntityTypeConfiguration<FocusedWarehouse>
    {
        public FocusedWarehouseMap()
        {
            ToTable("Warehouses");
        }
    }

    public class WarehouseMap : EntityTypeConfiguration<Warehouse>
    {
        
    }

    public class SalesTerritoryMap : EntityTypeConfiguration<SalesTerritory>
    {
        
    }
}