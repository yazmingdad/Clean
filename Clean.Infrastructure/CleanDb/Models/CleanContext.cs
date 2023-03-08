
using Clean.Infrastructure.CleanDb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clean.Infrastructure.CleanDb.Models
{
    public partial class CleanContext :DbContext
    {
        public DbSet<Card> Cards { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentType> DepartmentTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<User> Users { get; set; }
        public CleanContext(DbContextOptions<CleanContext> options) : base(options)
        {}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<DepartmentType>(dt => {
                dt.HasKey(p => p.Id);
                dt.Property(p => p.Name).HasColumnType("nvarchar(100)").IsRequired();
                dt.ToTable("DepartmentTypes");
            });

            modelBuilder.Entity<Rank>(r => {
                r.HasKey(p => p.Id);
                r.Property(p => p.Name).HasColumnType("nvarchar(200)").IsRequired();
                r.ToTable("Ranks");
            });

            modelBuilder.Entity<Country>(c => {
                c.HasKey(p => p.Id);
                c.Property(p => p.Name).HasColumnType("nvarchar(200)").IsRequired();
                c.ToTable("Countries");
            });

            modelBuilder.Entity<City>(c => {
                c.HasKey(p => p.Id);
                c.Property(p => p.Name).HasColumnType("nvarchar(100)").IsRequired();
                c.Property(p => p.Latitude).HasColumnType("nvarchar(25)").IsRequired();
                c.Property(p => p.Longitude).HasColumnType("nvarchar(25)").IsRequired();
                c.HasOne<Country>().WithMany().HasForeignKey(c => c.CountryId).IsRequired();
                c.ToTable("Cities");
            });

            modelBuilder.Entity<Card>(c => {
                c.HasKey(p => p.Id);
                c.Property(p => p.Number).HasColumnType("nvarchar(40)").IsRequired();
                c.HasOne<Employee>().WithMany().HasForeignKey(c => c.EmployeeId).IsRequired();
                c.ToTable("Cards");
            });

            modelBuilder.Entity<User>(u => {
                u.HasKey(p => p.Id);
                u.HasOne<Employee>().WithOne().HasForeignKey<User>("EmployeeId").IsRequired();
                u.ToTable("Users");
            });

            modelBuilder.Entity<Department>(d => {
                d.HasKey(p => p.Id);
                d.Property(p => p.Name).HasColumnType("nvarchar(200)").IsRequired();
                d.Property(p => p.ShortName).HasColumnType("nvarchar(20)").IsRequired();
                d.Property(p => p.IsDown);
                d.HasOne<City>().WithMany().HasForeignKey(d => d.CityId).IsRequired();
                d.HasOne<DepartmentType>().WithMany().HasForeignKey(d => d.DepartmentTypeId).IsRequired();
                d.HasOne<Department>().WithMany().HasForeignKey(d => d.ParentId);
                d.HasOne<Employee>().WithOne().HasForeignKey<Department>("ManagerId");
                d.ToTable("Departments");
            });

            modelBuilder.Entity<Employee>(em =>
            {
                em.HasKey(p => p.Id);
                em.Property(p => p.FirstName).HasColumnType("nvarchar(35)").IsRequired();
                em.Property(p => p.LastName).HasColumnType("nvarchar(35)").IsRequired();
                em.Property(p => p.SSN).HasColumnType("nvarchar(35)").IsRequired();
                em.Property(p => p.Avatar).IsRequired();
                em.Property(p => p.IsRetired);
                em.HasOne<Card>().WithOne().HasForeignKey<Employee>("ActiveCardId");
                em.HasOne<Rank>().WithMany().HasForeignKey(e => e.RankId).IsRequired();
                em.HasOne<Department>().WithMany().HasForeignKey(e => e.DepartmentId).IsRequired();
                em.ToTable("Employees");
            });


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            base.OnModelCreating(modelBuilder);

        }



    }
}
