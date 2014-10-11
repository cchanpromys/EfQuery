using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EfQuery
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        SystemStatus SystemStatus { get; set; }
    }

    public class SalesTerritory : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SystemStatus SystemStatus { get; set; }

        public virtual Warehouse Warehouse { get; set; }
    }

    public class Warehouse : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInternal { get; set; }
        public bool IsDefaultInternal { get; set; }
        public bool IsServicing { get; set; }
        public bool IsDefaultServicing { get; set; }
        public bool IsReceiving { get; set; }
        public bool IsDefaultReceiving { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefaultShipping { get; set; }
        public bool IsStaging { get; set; }
        public bool IsDefaultStaging { get; set; }
        public bool IsAdjustable { get; set; }

        public SystemStatus SystemStatus { get; set; }

        public virtual ICollection<SalesTerritory> SalesTerritories { get; set; }
    }

    public class FocusedWarehouse : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SystemStatus SystemStatus { get; set; }
    }

    public class Product : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public SystemStatus SystemStatus { get; set; }

        public virtual Warehouse Warehouse { get; set; }
        public virtual ICollection<QuoteDetail> QuoteDetails { get; set; }
    }

    public class QuoteDetail : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int Quality { get; set; }
        public int ParentId { get; set; }
        public int ProductId { get; set; }

        public SystemStatus SystemStatus { get; set; }

        public virtual Product Product { get; set; }
        public virtual Quote Quote { get; set; }
    }

    public class Quote : IBaseEntity
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public SystemStatus SystemStatus { get; set; }

        [NotMapped]
        public decimal Total
        {
            get { return QuoteDetails.Sum(x =>  x.Price*x.Quality); }
        }

        public virtual ICollection<QuoteDetail> QuoteDetails { get; set; }
    }

    public enum SystemStatus
    {
        Active = 0,
        Deleted = 1
    }
}