using System.Collections.Generic;

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
        public SystemStatus SystemStatus { get; set; }

        public virtual Quote Quote { get; set; }
    }

    public class Quote : IBaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SystemStatus SystemStatus { get; set; }

        public virtual ICollection<QuoteDetail> QuoteDetails { get; set; }
    }

    public enum SystemStatus
    {
        Active = 0,
        Deleted = 1
    }
}