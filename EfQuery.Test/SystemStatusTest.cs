using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace EfQuery.Test
{
    [TestFixture]
    public class SystemStatusTest
    {
        [Test]
        public void CanSoftDelete()
        {
            //Arrange
            var q = new Quote
                {
                    Name = "CanSoftDelete",
                    QuoteDetails = new Collection<QuoteDetail>
                        {
                            new QuoteDetail {Name = "Item 1"},
                            new QuoteDetail {Name = "Item 2"},
                        }
                };

            using (var db = new Context())
            {
                db.Quotes.Add(q);
                db.SaveChanges();
            }

            //Act
            using (var db = new Context())
            {
                q = db.Quotes.Single(x => x.Id == q.Id);
                db.Quotes.Remove(q);
                db.SaveChanges();
            }
        }
    }
}
