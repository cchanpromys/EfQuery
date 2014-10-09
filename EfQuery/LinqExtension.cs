using System.Linq;

namespace EfQuery
{
    public static class LinqExtension
    {
        public static IQueryable<Test.RegionOrganizationPair> DrillDown(
            this IQueryable<Test.RegionOrganizationPair> query, IQueryable<Region> regions)
        {
            return query
                .GroupJoin(regions,
                           pair => pair.Region.ParentId,
                           region => region.Id,
                           (x, y) => new
                               {
                                   x.Organization,
                                   Regions = y,
                                   x.Region,
                                   x.OrganizationLocation
                               })
                .Where(x => x.Regions.Any())
                .SelectMany(x => x.Regions,
                            (x, y) => new Test.RegionOrganizationPair
                                {
                                    Organization = x.Organization,
                                    Region = y,
                                    OrganizationLocation = x.OrganizationLocation
                                });
        }

        public static IQueryable<Test.RegionOrganizationPair> DrillDown(
            this IQueryable<Test.RegionOrganizationPair> query, IQueryable<Region> regions, int levels)
        {
            var result = query;
            var level = query.DrillDown(regions);
            result = result.Union(level);

            for (int i = 1; i < levels; i++)
            {
                level = level.DrillDown(regions);
                result = result.Union(level);
            }

            return result;
        }
    }
}