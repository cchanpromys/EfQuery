using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EfQuery.EfHelper;
using EfQuery.Mappings;

namespace EfQuery
{
    //[DbConfigurationType(typeof(EntityFrameworkConfiguration))] 
    public class Context : DbContext
    {
        public Context()
            : base("Data Source=localhost;Initial Catalog=EfQuery;Integrated Security=SSPI;")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder m)
        {
            m.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            var c = m.Configurations;

            c.Add(new QuoteMap());
            c.Add(new QuoteDetailMap());
            c.Add(new ProductMap());
            c.Add(new WarehouseMap());
            c.Add(new SalesTerritoryMap());
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteDetail> QuoteDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<SalesTerritory> SalesTerritories { get; set; }
    }

    [DbConfigurationType(typeof(EntityFrameworkConfiguration2))]
    public class Context2 : DbContext
    {
        public Context2()
            : base("Data Source=localhost;Initial Catalog=EfQuery;Integrated Security=SSPI;")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder m)
        {
            m.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            //Bounded context
            //http://msdn.microsoft.com/en-us/magazine/jj883952.aspx
            m.Ignore<Warehouse>();
            m.Ignore<SalesTerritory>();

            var c = m.Configurations;

            c.Add(new QuoteMap());
            c.Add(new QuoteDetailMap());
            c.Add(new ProductMap());
            c.Add(new FocusedWarehouseMap());
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteDetail> QuoteDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FocusedWarehouse> Warehouses { get; set; }
    }
    
    //public class EntityFrameworkConfiguration : DbConfiguration
    //{
        
    //}

    //http://msdn.microsoft.com/en-ca/data/jj680699.aspx
    //"Create only one DbConfiguration class for your application. This class specifies app-domain wide settings."
    public class EntityFrameworkConfiguration2 : DbConfiguration
    {
        public EntityFrameworkConfiguration2()
        {
            AddInterceptor(new SoftDeleteInterceptor());
        }
    }
}