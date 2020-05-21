using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestSiteMapAnalysis.Storage
{
    public static class DbInitializer
    {
        public static void Initialize(DataContext context)
        {
            context.Database.CreateIfNotExists();

            // Look for any students.
            if (context.SitemapResults.Any())
            {
                return;   // DB has been seeded
            }
        }
    }
}
