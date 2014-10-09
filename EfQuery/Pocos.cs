using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EfQuery
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public virtual Region Parent { get; set; }

        public virtual ICollection<Region> Children { get; set; }
        public virtual ICollection<TaxDetail> TaxDetails { get; set; }

        public Region()
        {
            Children = new Collection<Region>();
            TaxDetails = new Collection<TaxDetail>();
        }
    }

    public class TaxSchedule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<TaxScheduleDetail> Children { get; set; }
    }

    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int? DefaultLocation_Id { get; set; }

        public virtual OrganizationLocation DefaultLocation { get; set; }
        public virtual ICollection<OrganizationLocation> OrganizationLocations { get; set; }

        public Organization()
        {
            OrganizationLocations = new Collection<OrganizationLocation>();
        }
    }

    public class OrganizationLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int Organization_Id { get; set; }
        public virtual Region Region { get; set; }
        public virtual Organization Parent { get; set; }
    }

    public class TaxScheduleDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual TaxSchedule Parent { get; set; }
    }

    public class TaxDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<TaxScheduleDetail> TaxScheduleDetails { get; set; }
    }
}