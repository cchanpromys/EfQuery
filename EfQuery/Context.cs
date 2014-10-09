using System.Data.Entity;
using EfQuery.EfHelper;

namespace EfQuery
{
    public class Context : DbContext
    {
        public Context()
            : base("Data Source=localhost;Initial Catalog=EfQuery;Integrated Security=SSPI;")
        {
            
        }

        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteDetail> QuoteDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>()
                        .HasOptional(a => a.QuoteDetails)
                        .WithOptionalDependent()
                        .WillCascadeOnDelete(true);
        }
    }

    public class EntityFrameworkConfiguration : DbConfiguration
    {
        public EntityFrameworkConfiguration()
        {
            AddInterceptor(new SoftDeleteInterceptor());
        }
    }
}