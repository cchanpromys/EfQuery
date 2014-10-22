using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using EntityFramework.BulkInsert.Extensions;
using NUnit.Framework;

namespace EfQuery.Test
{
    [TestFixture]
    public class QueryTest
    {
        const int TotalQuotes = 1000;
        const int NumOfLineItemsPerQuote = 100;
        const int PageSize = 5000;

        class Report
        {
            public string Name { get; set; }
            public decimal Total { get; set; }
        }

        //TODO: Demo how fast we can sum and multiply numbers
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
                                    .Include("QuoteDetails")
                                    .OrderBy(x => x.Id)
                                    .Skip(i*PageSize).Take(PageSize)
                                    .ToList()
                                    .Select(x => new Report
                                        {
                                            Name = x.Name,
                                            Total = x.Total
                                        });

                    result.AddRange(reports);
                }

                sw.Stop();

                var numQ = result.Count();
                var numQdPerQ = db.Quotes.First().QuoteDetails.Count;

                Console.WriteLine(
                    "Took {0}s to process {1} Quotes. Each of them contains {2} line items. We did {3} addition and {3} multiplication all in memory.",
                    sw.ElapsedMilliseconds/1000.0,
                    numQ,
                    numQdPerQ,
                    numQ*numQdPerQ);
            }

            //foreach (var r in result)
            //    Console.WriteLine("{0} - ${1}", r.Name, r.Total);
        }

        [Test]
        public void SelectnPlusOne()
        {
            var name = Guid.NewGuid().ToString();
            using (var db = new Context())
            {
                var q = new Quote {Name = name, QuoteDetails = new Collection<QuoteDetail>()};

                for (int i = 0; i < 500; i++)
                {
                    var w = new Warehouse();
                    var p = new Product { Warehouse = w };
                    q.QuoteDetails.Add(new QuoteDetail {Product = p});

                    db.Warehouses.Add(w);
                    db.Products.Add(p);
                }

                db.Quotes.Add(q);
                db.SaveChanges();
            }

            //TODO: Select N plus one when you select Warehouse. Check Profiler
            using (var db = new Context())
            {
                var sw = Stopwatch.StartNew();
                var q = db.Quotes.Single(x => x.Name == name);

                var warehouses = new List<Warehouse>();

                foreach(var qd in q.QuoteDetails)
                {
                    warehouses.Add(qd.Product.Warehouse);
                }

                sw.Stop();
                Console.WriteLine("Took {0} sec", sw.ElapsedMilliseconds/1000.0);
            }


            using (var db = new Context())
            {
                var sw = Stopwatch.StartNew();
                var q = db.Quotes
                          .Where(x => x.Name == name)
                          .Include(x => x.QuoteDetails.Select(y => y.Product.Warehouse))
                          .Single();

                var warehouses = new List<Warehouse>();
                foreach (var qd in q.QuoteDetails)
                {
                    warehouses.Add(qd.Product.Warehouse);
                }

                sw.Stop();
                Console.WriteLine("Took {0} sec", sw.ElapsedMilliseconds / 1000.0);
            }

        }

        [Test]
        public void InsertTonsOfData()
        {
            using (var db = new Context())
            {
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Configuration.ValidateOnSaveEnabled = false;

                var w = new Warehouse { Name = Guid.NewGuid().ToString() };

                db.Warehouses.Add(w);

                var p = new Product
                {
                    Name = Guid.NewGuid().ToString(),
                    Warehouse = w
                };

                db.Products.Add(p);

                db.SaveChanges();

                var quotes = new List<Quote>();
                var qds = new List<QuoteDetail>();

                for (int i = 1; i <= TotalQuotes; i++)
                {
                    var q = new Quote
                        {
                            Name = "Quote " + i,
                            QuoteDetails = new Collection<QuoteDetail>()
                        };

                    for (int j = 1; j <= NumOfLineItemsPerQuote; j++)
                    {
                        var qd = new QuoteDetail
                            {
                                Name = "Line Item " + j,
                                Quality = j,
                                Price = 10*j,
                                Cost = 20*j,
                                Product = p
                            };

                        q.QuoteDetails.Add(qd);
                        qds.Add(qd);
                        qd.Quote = q;
                        //db.QuoteDetails.Add(qd);
                    }

                    quotes.Add(q);
                    if (i%500 == 0)
                    {
                        db.Quotes.AddRange(quotes);
                        //db.BulkInsert(quotes);
                        //db.SaveChanges();
                        //db.BulkInsert(qds);
                        db.SaveChanges();
                        quotes.Clear();
                        Console.WriteLine("Flushed");
                        //qds.Clear();
                    }

                    //db.Quotes.Add(q);

                    //Flush every 500 records
                    //if (i%500 == 0)
                    //    db.SaveChanges();
                }

                db.Quotes.AddRange(quotes);
                //db.BulkInsert(quotes);
                //db.SaveChanges();
                //db.BulkInsert(qds);
                db.SaveChanges();
               
            }
        }
    }
}