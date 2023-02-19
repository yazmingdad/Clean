using Clean.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Clean.Infrastructure.Identity.Design
{
    public class DesignDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string path = Directory.GetCurrentDirectory();

            IConfigurationBuilder builder =
                new ConfigurationBuilder()
                    .SetBasePath(path)
                    .AddJsonFile("appsettings.json");

            IConfigurationRoot config = builder.Build();

            string connectionString = config.GetConnectionString("CleanIdentity");

            Console.WriteLine($"DesignDbContextFactory: using base path = {path}");
            Console.WriteLine($"DesignDbContextFactory: using connection string = {connectionString}");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could not find connection string named 'CleanIdentity'");
            }

            DbContextOptionsBuilder<ApplicationDbContext> dbContextOptionsBuilder =
                new DbContextOptionsBuilder<ApplicationDbContext>();

            ApplicationDbContext.AddBaseOptions(dbContextOptionsBuilder, connectionString);

            return new ApplicationDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
