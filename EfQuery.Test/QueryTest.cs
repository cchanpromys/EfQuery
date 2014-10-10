using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace EfQuery.Test
{
    [TestFixture]
    public class QueryTest
    {
        const int TotalQuotes = 10000;
        const int NumOfLineItemsPerQuote = 10;
        const int PageSize = 50000;

        class Report
        {
            public string Name { get; set; }
            public decimal Total { get; set; }
        }

        [Test]
        public void DynamicSum()
        {
            var result = new List<Report>();

            using (var db = new Context())
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i <= TotalQuotes/PageSize; i++)
                {
                    var reports = db.Quotes
                                    .OrderBy(x=> x.Id)
                                    .Skip(i*PageSize).Take(PageSize)
                                    .Select(x => new Report
                                        {
                                            Name = x.Name,
                                            Total = x.QuoteDetails.Sum(y => y.Quality*y.Price)
                                        });

                    result.AddRange(reports);
                }
                sw.Stop();
                Console.WriteLine("Took {0}s", sw.ElapsedMilliseconds/1000.0);
            }



            //foreach (var r in result)
            //    Console.WriteLine("{0} - ${1}", r.Name, r.Total);
        }

        [Test]
        public void ViewModelSum()
        {
            var result = new List<Report>();

            using (var db = new Context())
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i <= TotalQuotes / PageSize; i++)
                {
                    var reports = db.Quotes
                                    .AsNoTracking()
                                    .Include("QuoteDetails")
                                    .OrderBy(x => x.Id)
                                    .Skip(i * PageSize).Take(PageSize)
                                    .ToList()
                                    .Select(x => new Report
                                    {
                                        Name = x.Name,
                                        Total = x.Total
                                    });

                    result.AddRange(reports);
                }
                sw.Stop();
                Console.WriteLine("Took {0}s", sw.ElapsedMilliseconds / 1000.0);
            }

            //foreach (var r in result)
            //    Console.WriteLine("{0} - ${1}", r.Name, r.Total);
        }

        [Test]
        public void InsertTonsOfData()
        {
            using (var db = new Context())
            {
                for (int i = 1; i <= TotalQuotes; i++)
                {
                    var q = new Quote
                        {
                            Name = "Quote " + i,
                            QuoteDetails = new Collection<QuoteDetail>()
                        };

                    for (int j = 1; j <= NumOfLineItemsPerQuote; j++)
                    {
                        q.QuoteDetails.Add(new QuoteDetail {Name = "Line Item " + j, Quality = j, Price = 10*j});
                    }

                    db.Quotes.Add(q);

                    //Flush every 500 records
                    if (i%500 == 0)
                        db.SaveChanges();
                }
            }
        }
    }
}