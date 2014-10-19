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
    }
}
