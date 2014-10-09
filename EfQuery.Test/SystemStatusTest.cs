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
            int id;
            using (var db = new Context())
            {
                var q = new Quote
                {
                    Name = "CanSoftDelete",
                    QuoteDetails = new[]
                        {
                            new QuoteDetail {Name = "Item 1"},
                            new QuoteDetail {Name = "Item 2"}
                        }
                };

                db.Quotes.Add(q);
                db.SaveChanges();
                id = q.Id;
            }

            //Act
            using (var db = new Context())
            {
                var q = db.Quotes.Single(x => x.Id == id);
                db.Quotes.Remove(q);
                db.SaveChanges();
            }

            //Assert
            using (var db = new Context())
            {
                Assert.That(db.Quotes.Any(x => x.Id == id), Is.False);
            }
        }

        [Test]
        public void ChildrenSupportSoftDelete()
        {
            //Arrange
            int id;
            using (var db = new Context())
            {
                var q = new Quote
                {
                    Name = "CanSoftDeleteChildren",
                    QuoteDetails = new[]
                        {
                            new QuoteDetail {Name = "Item 1"},
                            new QuoteDetail {Name = "Remove Me"}
                        }
                };

                db.Quotes.Add(q);
                db.SaveChanges();
                id = q.Id;
            }

            //Act
            using (var db = new Context())
            {
                var q = db.Quotes.Single(x => x.Id == id);
                var qd = q.QuoteDetails.Single(x => x.Name == "Remove Me");
                db.QuoteDetails.Remove(qd);
                db.SaveChanges();
            }

            //Assert
            using (var db = new Context())
            {
                var q = db.Quotes.Single(x => x.Id == id);
                Assert.That(q.QuoteDetails.Any(x => x.Name == "Remove Me"), Is.False);
                Assert.That(q.QuoteDetails.Count(), Is.EqualTo(1));
            }
        }
    }
}
