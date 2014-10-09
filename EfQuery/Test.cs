using System;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace EfQuery
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void Flushing()
        {
            using (var db = new Context())
            {
                var foo = new Organization
                    {
                        Name = "Foo"
                    };

                db.Organizations.Add(foo);
                var orgLocation = new OrganizationLocation
                    {
                        Name = "Foo Location"
                    };

                foo.OrganizationLocations.Add(orgLocation);

                db.SaveChanges();
            }
        }

        [Test]
        public void Clone()
        {
            using (var db = new Context())
            {
                //var orgLoc = db.OrganizationLocations.First();
                //var clone = MakeShallowCopy(orgLoc);
                //clone.Name = "Clone " + clone.Name;

                var clone = new OrganizationLocation {Id = 0, Name = "Cloned"};
                
                var foo = new Organization
                    {
                        Name = "Cloning Org",
                        OrganizationLocations = new Collection<OrganizationLocation> {clone},
                        DefaultLocation = clone
                    };

                db.Organizations.Add(foo);
                db.OrganizationLocations.Add(clone);
                db.SaveChanges();
            }
        }

        [Test]
        public void InsertAndUpdateTest()
        {
            using (var db = new Context())
            {
                var foo = new Organization
                {
                    Name = "Foo"
                };

                db.Organizations.Add(foo);

                db.SaveChanges();

                foo.Name = "bar";

                db.SaveChanges();
            }
        }


        [Test]
        public void InsertRecur()
        {
            using (var db = new Context())
            {
                var turtle = new Organization
                    {
                        Name = "Snapping Turtle"
                    };

                db.Organizations.Add(turtle);



                var us = new Region
                    {
                        Name = "US"
                    };

                var canada = new Region
                    {
                        Name = "Canada",
                    };

                var estCanada = new Region
                    {
                        Name = "Eastern Canada",
                        Parent = canada,
                    };

                var ontario = new Region
                    {
                        Name = "Ontario",
                        Parent = estCanada,
                    };

                var mississauga = new Region
                    {
                        Name = "Mississauga",
                        Parent = ontario,
                    };

                canada.Children.Add(estCanada);
                estCanada.Children.Add(ontario);
                ontario.Children.Add(mississauga);

                db.Regions.Add(canada);
                db.Regions.Add(us);

                var ontarioTax = new TaxSchedule
                    {
                        Name = "Ont Tax",
                        Region = ontario
                    };

                var canadaTax = new TaxSchedule
                    {
                        Name = "Canada Tax",
                        Region = canada
                    };

                db.TaxSchedules.Add(ontarioTax);
                db.TaxSchedules.Add(canadaTax);

                var ontarioTax1 = new TaxScheduleDetail
                    {
                        Name = "HST",
                        Parent = ontarioTax
                    };

                var canadaTax1 = new TaxScheduleDetail
                    {
                        Name = "GST",
                        Parent = canadaTax
                    };

                db.TaxScheduleDetails.Add(ontarioTax1);
                db.TaxScheduleDetails.Add(canadaTax1);

                var taxDetailForOntario = new TaxDetail
                    {
                        Name = "Ont Tax Detail",
                        TaxScheduleDetails = new Collection<TaxScheduleDetail> {ontarioTax1}
                    };

                var taxDetailForCanada = new TaxDetail
                    {
                        Name = "Canada Tax Detail",
                        TaxScheduleDetails = new Collection<TaxScheduleDetail> {canadaTax1}
                    };

                db.TaxDetails.Add(taxDetailForOntario);
                db.TaxDetails.Add(taxDetailForCanada);

                ontario.TaxDetails.Add(taxDetailForOntario);
                canada.TaxDetails.Add(taxDetailForCanada);

                var mississaugaLocation = new OrganizationLocation
                    {
                        Name = "Mississuaga Location",
                        Region = mississauga
                    };

                var usLocation = new OrganizationLocation
                    {
                        Name = "US Location",
                        Region = us
                    };

                turtle.OrganizationLocations.Add(mississaugaLocation);
                turtle.OrganizationLocations.Add(usLocation);

                db.OrganizationLocations.Add(mississaugaLocation);
                db.OrganizationLocations.Add(usLocation);

                db.SaveChanges();
            }
        }

        public class ViewModel
        {
            public string OrganizationName { get; set; }
            public string RegionName { get; set; }
            public string TaxScheduleName { get; set; }
            public string OrganizationLocation { get; set; }
        }

        public class RegionOrganizationPair
        {
            public Organization Organization { get; set; }
            public OrganizationLocation OrganizationLocation { get; set; }
            public Region Region { get; set; }
        }

        [Test]
        public void SelectRecur()
        {
            using (var db = new Context())
            {
                var regions = db.Regions;

                var result =
                    db
                        .Organizations
                        .SelectMany(x => x.OrganizationLocations.Select(y => new { y.Region, OrganizationLocation = y}),
                                    (o, p) => new RegionOrganizationPair
                                        {
                                            Organization = o,
                                            Region = p.Region,
                                            OrganizationLocation =  p.OrganizationLocation
                                        })
                        .DrillDown(regions, 4)
                        .SelectMany(
                            x => x.Region.TaxDetails.SelectMany(z => z.TaxScheduleDetails.Select(y => y.Parent)),
                            (p, ts) => new
                                {
                                    p.Organization,
                                    p.Region,
                                    TaxSchedule = ts,
                                    p.OrganizationLocation
                                })
                        .Select(x => new ViewModel
                            {
                                OrganizationName = x.Organization.Name,
                                RegionName = x.Region.Name,
                                TaxScheduleName = x.TaxSchedule.Name,
                                OrganizationLocation = x.OrganizationLocation.Name
                            });

                foreach (var r in result)
                {
                    Console.WriteLine("| {0} | {1} | {2} | {3} |", r.OrganizationName, r.OrganizationLocation, r.RegionName,
                                      r.TaxScheduleName);
                }
            }
        }



        public static T MakeShallowCopy<T>(T source) where T : new()
        {
            var theCopy = new T();

            // skipp all non-standard types and collections
            var properties = theCopy.GetType().GetProperties()
                .Where(p => p.PropertyType.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase)
                    && !p.PropertyType.FullName.StartsWith("System.Collections", StringComparison.OrdinalIgnoreCase));

            var fields = theCopy.GetType().GetFields()
                .Where(f => f.FieldType.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase)
                    && !f.FieldType.FullName.StartsWith("System.Collections", StringComparison.OrdinalIgnoreCase));

            foreach (var property in properties)
            {
                try
                {

                    if (property.GetSetMethod() != null && property.GetGetMethod() != null)
                    {
                        if (property.PropertyType.FullName.StartsWith("System.String", StringComparison.OrdinalIgnoreCase))
                        {
                            var val = String.Copy((string)property.GetValue(source, null));
                            property.SetValue(theCopy, val, null);
                        }
                        else if (property.PropertyType.IsValueType)
                        {
                            property.SetValue(theCopy, property.GetValue(source, null), null);
                        }
                    }
                }
                catch { }
            }

            foreach (var field in fields)
            {
                try
                {
                    if (field.IsPublic)
                    {
                        if (field.FieldType.FullName.StartsWith("System.String", StringComparison.OrdinalIgnoreCase))
                        {
                            var val = String.Copy((string)field.GetValue(source));
                            field.SetValue(theCopy, val);
                        }
                        else if (field.FieldType.IsValueType)
                        {
                            field.SetValue(theCopy, field.GetValue(source));
                        }
                    }
                }
                catch { }
            }

            return theCopy;
        }
    }
}