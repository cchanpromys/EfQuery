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

    public class QuoteDetail : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quality { get; set; }

        public SystemStatus SystemStatus { get; set; }

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