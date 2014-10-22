using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace EfQuery.Test
{
    [TestFixture]
    public class SystemStatusTest
    {
        //TODO: This is to demo that running "Remove()" will soft delete an entity
        //Without this, we will have to do .Where(x=> x.SystemStatus == SystemStatus.Active).Sum(x=> x.blah)
        [Test]
        public void CanSoftDelete()
        {
            //Arrange
            int id;
            using (var db = new Context2())
            {
                var p = new Product();
                
                db.Products.Add(p);

                var q = new Quote
                {
                    Name = "CanSoftDelete",
                    QuoteDetails = new[]
                        {
                            new QuoteDetail {Name = "Item 1", Product = p},
                            new QuoteDetail {Name = "Item 2", Product = p}
                        }
                };

                db.Quotes.Add(q);
                db.SaveChanges();
                id = q.Id;
            }

            //Act
            using (var db = new Context2())
            {
                var q = db.Quotes.Single(x => x.Id == id);
                db.Quotes.Remove(q);
                db.SaveChanges();
            }

            //Assert
            using (var db = new Context2())
            {
                Assert.That(db.Quotes.Any(x => x.Id == id), Is.False);
            }
        }

        [Test]
        public void SetIdManually()
        {
            int qdId;
            using (var db = new Context())
            {
                var q = new Quote
                    {
                        QuoteDetails = new Collection<QuoteDetail>()
                    };

                var p = new Product();

                var qd = new QuoteDetail
                    {
                        Name = "blah",
                        Product = p
                    };
                db.Products.Add(p);
                db.QuoteDetails.Add(qd);
                db.Quotes.Add(q);

                db.SaveChanges();

                qdId = qd.Id;
            }

            using (var db = new Context())
            {
                var qd = db.QuoteDetails.Single(x => x.Id == qdId);

                qd.ParentId = 6;

                db.ChangeTracker.DetectChanges();

                Assert.That(qd.Quote.Id == 6);
            }
        }
    }
}
