using System.Data.Entity;

namespace EfQuery
{
    public class Context : DbContext
    {
        public Context()
            : base("Data Source=localhost;Initial Catalog=EfQuery;Integrated Security=SSPI;")
        {
            
        }

        public DbSet<TaxDetail> TaxDetails { get; set; }
        public DbSet<TaxScheduleDetail> TaxScheduleDetails { get; set; }
        public DbSet<TaxSchedule> TaxSchedules { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationLocation> OrganizationLocations { get; set; }
        public DbSet<Region> Regions { get; set; }
        
    }
}