using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Entities
{
    public class CleanDbContext : DbContext
    {
        public CleanDbContext(DbContextOptions<CleanDbContext> options):base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}
