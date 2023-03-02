
using Clean.Infrastructure.CleanDb.Models;
using Clean.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Design
{
    public class DesignDbContextFactory : IDesignTimeDbContextFactory<CleanContext>
    {
        public CleanContext CreateDbContext(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("Clean");

            Console.WriteLine($"DesignDbContextFactory: using base path = {path}");
            Console.WriteLine($"DesignDbContextFactory: using connection string = {connectionString}");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find connection string named 'Clean'");
            }

            DbContextOptionsBuilder<CleanContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<CleanContext>();

            CleanContext.AddBaseOptions(dbContextOptionsBuilder, connectionString);

            return new CleanContext(dbContextOptionsBuilder.Options);
        }
    }
}
